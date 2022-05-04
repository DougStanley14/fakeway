using Serilog;
using Serilog.Events;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Collections;

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

    // builder.Configuration.AddUserSecrets(theEnv.SecretsId);

    builder.WebHost.ConfigureKestrel(opt =>
        opt.ListenAnyIP(443, lstnOpt => {

            Log.Information($"       Cert File: '{theEnv.CertPath}'");
            Log.Information($"       Cert Pass: '{theEnv.CertPassword}'");
            Log.Information($"Cert File Exists: '{File.Exists(theEnv.CertPath)}'");

            lstnOpt.UseHttps(theEnv.CertPath, theEnv.CertPassword);
        })
    );

    Log.Information($"Configuring Ocelot with ocelot.{builder.Environment.EnvironmentName}.json");

    //builder.Services.AddEndpointsApiExplorer();
    //builder.Services.AddSwaggerGen();
    builder.Services.AddOcelot();

    builder.Host.UseSerilog((ctx, lc) => lc
            .MinimumLevel.Verbose()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)  // TODO: Integrate Logging Levels with Config File
            .Enrich.FromLogContext()
            .WriteTo.Console());

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    //if (app.Environment.IsDevelopment())
    //{
    //    app.UseSwagger();
    //    app.UseSwaggerUI();
    //}

    //app.UseHttpsRedirection();

    //app.UseHsts();

    //app.UseAuthorization();

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


