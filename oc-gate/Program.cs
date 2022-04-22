using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .WriteTo.Console()
    .CreateBootstrapLogger();

Log.Information("Starting up Ocelot Gateway");

try
{
    // TODO: Rewrite with newer (.net 6) Builder
    new WebHostBuilder()
        .UseKestrel()
        .UseContentRoot(Directory.GetCurrentDirectory())
        .UseSerilog((_, config) =>
        {
           config
            .MinimumLevel.Debug()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Debug)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .WriteTo.Seq("http://localhost:5109/");   // TODO: Put in Config File
           })
        .ConfigureAppConfiguration((hostingContext, config) =>
        {
           config
               .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
               .AddJsonFile("appsettings.json", true, true)
               .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
               .AddJsonFile($"ocelot.{hostingContext.HostingEnvironment.EnvironmentName}.json")
               .AddEnvironmentVariables();
        })
        .ConfigureServices(s =>
        {
           s.AddOcelot();
        })
        .UseIISIntegration()
        .Configure(app =>
        {
               //app.UseMiddleware<RequestResponseLoggingMiddleware>(); //https://www.abhith.net/blog/dotnet-core-api-gateway-ocelot-logging-http-requests-response-including-headers-body/
               app.UseOcelot().Wait();
               app.UseSerilogRequestLogging(); // May be replacement for above
           })
        .Build()
        .Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Ocelot Gateway - Unhandled exception");
}
finally
{
    Log.Information("Ocelot Gateway Shut down complete");
    Log.CloseAndFlush();
}