using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using microsvc_filemeta.AuthPolicies;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)  // Throttles EF Logging
                    .Enrich.FromLogContext()
                    .Enrich.WithExceptionDetails()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                    .WriteTo.Console()
                    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddSwaggerGen();

    //builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(o =>
    {
        //o.Authority = builder.Configuration["Jwt:Authority"];//http://localhost:5038/api/Authorization/verify
        //o.Authority = "http://localhost:5038/api/Authorization/verify";
        //o.Audience = "NDDS";
        //o.Audience = builder.Configuration["Jwt:Audience"];
        //o.RequireHttpsMetadata = false;
        var sskeysecret = builder.Configuration["Jwt:TwoGuidSecret"];
        //var sskeysecret = "{7445AF03-2612-4C82-A918-47215729FF9B}{41AAEA73-5C8C-4ADF-9A83-E6731FA00BF5}";
        o.TokenValidationParameters = new TokenValidationParameters
        {
            //ValidateIssuer = false,
            //ValidateAudience = false,
            //ValidateIssuerSigningKey = false,
            //ValidateLifetime = false,
            //RequireExpirationTime = false,
            //RequireSignedTokens = false

            // The Real Stuff Prior to Ignore (above)
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sskeysecret)),
            ValidateIssuer = false,
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

    builder.Services.AddAuthorization(o =>
    {;
        o.AddPolicy("FileMetaTestPolicy", policy =>
            policy.Requirements.Add(new MeetsPlatformRequirement())
            );
    });

    builder.Services.AddSingleton<IAuthorizationHandler, FileMetaAuthorizationHandler>();

    builder.Host.UseSerilog((ctx, lc) => lc
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)  // TODO: Integrate Logging Levels with Config File
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console());

    var app = builder.Build();

    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        IdentityModelEventSource.ShowPII = true;
    }

    // app.UseHttpsRedirection();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "App Failed to Start");
}
