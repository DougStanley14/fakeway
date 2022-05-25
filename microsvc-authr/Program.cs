using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using microsvc_authr;
using microsvc_authr.Data;
using microsvc_authr.Data.Seeders;
using microsvc_authr.Model;
using microsvc_authr.Services;
using Serilog;
using Serilog.Events;
using Serilog.Exceptions;
using System;
using System.Security.Claims;

Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)  // Throttles EF Logging
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                    .Enrich.WithExceptionDetails()
                    .WriteTo.Console()
                    .CreateBootstrapLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Services.AddControllers();
    builder.Services.AddEndpointsApiExplorer();
    builder.Services.AddDbContext<AuthRContext>(opt => opt.UseInMemoryDatabase("AuthInMem"));
    builder.Services.AddScoped<ITokenBuilder, TokenBuilder>();
    builder.Services.AddSwaggerGen();
    builder.Services.AddTransient<IClaimsTransformation, ClaimsTransformer>();
    builder.Services.AddTransient<IUserProfileService, UserProfileService>();
    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

    }).AddJwtBearer(o =>
    {

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
            }
        };
    });

    builder.Host.UseSerilog((ctx, lc) => lc
                .MinimumLevel.Verbose()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)  // TODO: Integrate Logging Levels with Config File
                .Enrich.FromLogContext()
                .Enrich.WithExceptionDetails()
                .WriteTo.Console());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI();
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
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var db = new AuthRContext(
            serviceProvider.GetRequiredService<DbContextOptions<AuthRContext>>()))
        {
            // Look for any board games.
            if (db.Users.Any())
            {
                return;   // Data was already seeded
            }

            var csvFilePath = @"DummyBunoSample.csv";
            var prsr = new BunoDumpParser(csvFilePath);
            prsr.ParseBuno();

            db.Users.AddRange(LookupSeeds.NddsUsers());
            prsr.Platforms.ForEach(p => p.Id = 0);
            db.Platforms.AddRange(prsr.Platforms);
            db.SaveChanges();
            foreach (var org in prsr.WMSavers)
            {
                db.ParentOrgs.Add(org);

                var orgplats = org.Orgs.SelectMany(o => o.OrgPlatforms)
                                           .ToList();

                db.SaveChanges();
                Console.WriteLine($"Added ParentOrg {org.LongName}");
            }

            db.UserOrgs.AddRange(GenUsersInGroups(db));
            db.SaveChanges();
        }
    }

    private static List<UserOrg> GenUsersInGroups(AuthRContext db)
    {
        var usgs = new List<UserOrg>();

        var gpname = db.Organizations.GroupBy(x => x.Name).Select(g => g.Key).ToList();

        // User 1
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "A41")
            .Select(s => new UserOrg { UserId = 1, OrgId = s.Id })
            );
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "Q65")
            .Select(s => new UserOrg { UserId = 1, OrgId = s.Id })
            );
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "GE7")
            .Select(s => new UserOrg { UserId = 1, OrgId = s.Id })
            );

        // User 2
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "SD2")
            .Select(s => new UserOrg { UserId = 2, OrgId = s.Id })
            );
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "SE4")
            .Select(s => new UserOrg { UserId = 2, OrgId = s.Id })
            );

        // User 3
        usgs.AddRange(
            db.Organizations.Where(s => s.Name == "GEY")
            .Select(s => new UserOrg { UserId = 3, OrgId = s.Id })
            );

        return usgs;
    }
}
