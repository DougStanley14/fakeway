using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using microsvc_authr;
using microsvc_authr.IdentModel;
using OpenIddict.Abstractions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<DefaultDbContext>(options =>
{
    //options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection"));
    options.UseSqlite("Data Source=demoDb.db");

    options.UseOpenIddict();
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.ClaimsIdentity.UserNameClaimType = OpenIddictConstants.Claims.Name;
    options.ClaimsIdentity.UserIdClaimType = OpenIddictConstants.Claims.Subject;
    options.ClaimsIdentity.RoleClaimType = OpenIddictConstants.Claims.Role;
    // configure more options if necessary...
});

builder.Services.AddOpenIddict()
                .AddCore(options => options.UseEntityFrameworkCore().UseDbContext<DefaultDbContext>())
                .AddServer(options =>
                {
                    // Enable the required endpoints
                    options.SetTokenEndpointUris("/connect/token");
                    options.SetUserinfoEndpointUris("/connect/userinfo");

                    options.AllowPasswordFlow();
                    options.AllowRefreshTokenFlow();
                    // Add all auth flows that you want to support
                    // Supported flows are:
                    //      - Authorization code flow
                    //      - Client credentials flow
                    //      - Device code flow
                    //      - Implicit flow
                    //      - Password flow
                    //      - Refresh token flow

                    // Custom auth flows are also supported
                    options.AllowCustomFlow("custom_flow_name");

                    // Using reference tokens means that the actual access and refresh tokens are stored in the database
                    // and a token referencing the actual tokens (in the db) is used in the request header.
                    // The actual tokens are not made public.
                    options.UseReferenceAccessTokens();
                    options.UseReferenceRefreshTokens();

                    // Register your scopes
                    // Scopes are a list of identifiers used to specify what access privileges are requested.
                    options.RegisterScopes(OpenIddictConstants.Permissions.Scopes.Email,
                                                OpenIddictConstants.Permissions.Scopes.Profile,
                                                OpenIddictConstants.Permissions.Scopes.Roles);

                    // Set the lifetime of your tokens
                    options.SetAccessTokenLifetime(TimeSpan.FromMinutes(30));
                    options.SetRefreshTokenLifetime(TimeSpan.FromDays(7));

                    // Register signing and encryption details
                    options.AddDevelopmentEncryptionCertificate()
                                    .AddDevelopmentSigningCertificate();

                    // Register ASP.NET Core host and configure options
                    options.UseAspNetCore().EnableTokenEndpointPassthrough();
                })
                .AddValidation(options =>
                {
                    options.UseLocalServer();
                    options.UseAspNetCore();
                });

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = OpenIddictConstants.Schemes.Bearer;
    options.DefaultChallengeScheme = OpenIddictConstants.Schemes.Bearer;
});

builder.Services.AddIdentity<User, Role>()
                .AddSignInManager()
                .AddUserStore<UserStore>()
                .AddRoleStore<RoleStore>()
                .AddUserManager<UserManager<User>>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
