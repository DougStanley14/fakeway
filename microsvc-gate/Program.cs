using Serilog;
using Serilog.Events;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Collections;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using Microsoft.IdentityModel.Logging;
using System.Security.Claims;
using System.Text;

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)  // Throttles EF Logging
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                    .WriteTo.Console()
                    .CreateLogger();

Log.Information("Starting NDDS Gateway");
LogCriticalVars();

try
{
    var builder = WebApplication.CreateBuilder(args);
    Log.Information($"ASPNETCORE_ENVIRONMENT - '{builder.Environment.EnvironmentName}'");
    var theEnv = new DockerEnv(builder);
    Log.Information("Docker Env {@denv}", theEnv);

    builder.Configuration.AddJsonFile("appsettings.json", true, true);
    builder.Configuration.AddJsonFile($"appsettings.{theEnv.ConfigName}.json", true, true);
    builder.Configuration.AddJsonFile($"ocelot.{theEnv.ConfigName}.json");

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(o =>
    {
        Log.Information($"Jwt:Authority = '{builder.Configuration["Jwt:Authority"]}'");
        Log.Information($"Jwt:Audience = '{builder.Configuration["Jwt:Audience"]}'");

        o.Authority = builder.Configuration["Jwt:Authority"]; // Currently From KeyCloak
        o.Audience = builder.Configuration["Jwt:Audience"];   // Currently From KeyCloak
        o.RequireHttpsMetadata = false;
        o.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateAudience = false
        };

        o.Events = new JwtBearerEvents
        {
            OnAuthenticationFailed = c =>
            {
                Log.Error(c.Exception, "XXX Auth Failed");
                c.NoResult();
                c.Response.StatusCode = 500;
                c.Response.ContentType = "text/plain";

                if (builder.Environment.IsDevelopment())
                {
                    return c.Response.WriteAsync(c.Exception.ToString());
                }

                return c.Response.WriteAsync("An error occured processing your authentication.");
            },
            OnMessageReceived = c =>
            {
                Log.Information($"JWT OnMessage Recieved {c.Token}");
                Log.Information("Request Heads {@heads}", c.HttpContext.Request.Headers);

                return Task.CompletedTask;
            },
            OnChallenge = c =>
            {
                Log.Information($"JWT OnChallenge {c.Scheme.Name}");
                Log.Information("Request Heads {@heads}", c.HttpContext.Request.Headers);

                return Task.CompletedTask;
            },
            OnTokenValidated = c =>
            {
                //Log.Information("JWT Validated {@token}", c.SecurityToken);

                var claims = ((JwtSecurityToken)c.SecurityToken).Claims.ToList();

                claims.ForEach(c => Log.Information("Claim {@claimpair}", new { c.Type, c.Value }));

                return Task.CompletedTask;
            }
        };
    });

    builder.WebHost.ConfigureKestrel(opt =>
        opt.ListenAnyIP(80, lstnOpt => {

            //Log.Information($"       Cert File: '{theEnv.CertPath}'");
            //Log.Information($"       Cert Pass: '{theEnv.CertPassword}'");
            //Log.Information($"Cert File Exists: '{File.Exists(theEnv.CertPath)}'");

            //lstnOpt.UseHttps(theEnv.CertPath, theEnv.CertPassword);
        })
    );

    Log.Information($"Configuring Ocelot with ocelot.{builder.Environment.EnvironmentName}.json");

    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
    builder.Services.AddOcelot()
                    .AddDelegatingHandler<AuthRHandler>(true);

    builder.Host.UseSerilog((ctx, lc) => lc
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)  // TODO: Integrate Logging Levels with Config File
            .Enrich.FromLogContext()
            .WriteTo.Console());

    var app = builder.Build();

    IdentityModelEventSource.ShowPII = true;

    app.UseAuthentication();
    app.UseOcelot().Wait();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "App Failed to Start");
}

void LogCriticalVars()
{
    var myvars = GetCriticalVars();

    myvars.ForEach(v => Log.Information("ENV: {@var}", v));
}

List<(string EnvVar, string VarVal)> GetCriticalVars()
{
    var allvars = new List<(string EnvVar, string VarVal)>();

    var allenv = Environment.GetEnvironmentVariables();

    foreach (DictionaryEntry de in Environment.GetEnvironmentVariables())
    {
        allvars.Add((EnvVar: de.Key?.ToString() ?? "", VarVal: de.Value?.ToString() ?? ""));
    }

    var myvars = allvars.Where(v => v.EnvVar.StartsWith("ASP"))
                        .ToList();
    return myvars;
}

public class DockerEnv
{
    public DockerEnv(WebApplicationBuilder builder)
    {
        SvcName = builder.Environment.ApplicationName;
        ConfigName = builder.Environment.EnvironmentName;
        SecretsId = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password") ?? "";
        CertPassword = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Password") ?? "";
        CertPath = Environment.GetEnvironmentVariable("ASPNETCORE_Kestrel__Certificates__Default__Path") ?? "";
    }
    public string? SvcName { get; set; }
    public string? SecretsId { get; set; }
    public string? ConfigName { get; set; }
    public string? CertPath { get; set; }
    public string? CertPassword { get; set; }
}

public class AuthRHandler: DelegatingHandler
{
    private readonly IHttpContextAccessor _httpCtx;

    public AuthRHandler(IHttpContextAccessor httpContextAccessor)
    {
        _httpCtx = httpContextAccessor;

        var user = _httpCtx.HttpContext.User;

    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        IEnumerable<string> headerValues;
        if (request.Headers.TryGetValues("Authorization", out headerValues))
        {
            string accessToken = GenerateToken(_httpCtx.HttpContext.User);


            request.Headers.Remove("Authorization");
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", accessToken);
            
        }

        return await base.SendAsync(request, cancellationToken);
    }

    private string GenerateToken(ClaimsPrincipal user)
    {
        string sskSecret = "{7445AF03-2612-4C82-A918-47215729FF9B}{41AAEA73-5C8C-4ADF-9A83-E6731FA00BF5}";
        var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sskSecret));
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
        var expirationDate = DateTime.UtcNow.AddDays(2);

        //var claims = await GetUserClaims(userName, edipi);

        var claims = new List<Claim> {
                new Claim("AuthR", "Good"),
            };

        var jwt = new JwtSecurityToken(issuer: "NDDS",
                                       audience: "NDDS",
                                       claims: claims,
                                       expires: expirationDate,
                                       signingCredentials: signingCredentials);

        var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

        return encodedJwt;
    }
}
