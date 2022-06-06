
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;
using microsvc_userprofile;
using microsvc_userprofile.Data;
using microsvc_userprofile.Data.Seeders;
using microsvc_userprofile.Services;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)  // Throttles EF Logging
                    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)  // Throttles EF Logging
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console()
                    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    Log.Information($"ASPNETCORE_ENVIRONMENT - '{builder.Environment.EnvironmentName}'");

    builder.Services.AddControllers();
    builder.Services.AddHttpContextAccessor();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddDbContext<AuthRContext>(opt => opt.UseInMemoryDatabase("AuthInMem"));
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
    builder.Services.AddTransient<IUserProfileService, UserProfileService>();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(o =>
    {
        var sskeysecret = builder.Configuration["Jwt:TwoGuidSecret"];
        o.TokenValidationParameters = new TokenValidationParameters
        {
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

    builder.Host.UseSerilog((ctx, lc) => lc
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)  // Throttles EF Logging
                .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Warning)  // Throttles EF Logging
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
        IdentityModelEventSource.ShowPII = true;
    }

    // app.UseHttpsRedirection();
    app.UseSerilogRequestLogging();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllers();

    // Seed InMem DB
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<AuthRContext>();
        InMemSeeder.Initialize(services);
    }

    app.Run();

}
catch (Exception ex)
{
    Log.Fatal(ex, "App Failed to Start");
}
finally
{
    Log.CloseAndFlush();
}

public class InMemSeeder
{
    public static async void Initialize(IServiceProvider serviceProvider)
    {
        using (var db = new AuthRContext(
            serviceProvider.GetRequiredService<DbContextOptions<AuthRContext>>()))
        {
            // Look for any board games.
            if (db.Users.Any())
            {
                return;   // Data was already seeded
            }

            var ldr = new AuthRDBLoader(db, @"DummyBunoSample.csv");

            await ldr.Load();
        }
    }
}

















//var builder = WebApplication.CreateBuilder(args);

//// Add services to the container.

//builder.Services.AddControllers();
//builder.Services.AddEndpointsApiExplorer();
//builder.Services.AddSwaggerGen();

//var app = builder.Build();

//// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
//    app.UseSwagger();
//    app.UseSwaggerUI();
//}

//app.UseAuthorization();

//app.MapControllers();

//app.Run();
