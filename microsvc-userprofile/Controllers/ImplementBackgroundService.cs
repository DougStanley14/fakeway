// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace microsvc_userprofile.Controllers
{
    public class ImplementBackgroundService : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                Console.WriteLine($"Respponse from Background Service - {DateTime.Now}");
                await Task.Delay(1000);
            }
        }
    }
}


