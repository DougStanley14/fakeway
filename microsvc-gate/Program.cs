using Serilog;
using Serilog.Events;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;


Log.Logger = new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .MinimumLevel.Override("Microsoft", LogEventLevel.Verbose)  // Throttles EF Logging
                    .Enrich.FromLogContext()
                    .Enrich.WithProperty("ApplicationName", typeof(Program).Assembly.GetName().Name)
                    .WriteTo.Console()
                    .CreateLogger();

Log.Information("Starting NDDS Gateway");

try
{
    var builder = WebApplication.CreateBuilder(args);


    builder.Configuration.AddJsonFile("appsettings.json", true, true);
    builder.Configuration.AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true, true);
    builder.Configuration.AddJsonFile($"ocelot.{builder.Environment.EnvironmentName}.json");


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



