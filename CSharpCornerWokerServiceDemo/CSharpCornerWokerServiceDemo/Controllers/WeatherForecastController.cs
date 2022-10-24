using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CSharpWorkerService;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace CSharpCornerWokerServiceDemo.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
       
        public IBackgroundTaskQueue _queue { get; }
        private readonly IServiceScopeFactory _serviceScopeFactory;
        public WeatherForecastController(IBackgroundTaskQueue queue, IServiceScopeFactory serviceScopeFactory)
        {
            _queue = queue;
            _serviceScopeFactory = serviceScopeFactory;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _queue.QueueBackgroundWorkItem(async token =>
            {
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    int j = 1000;
                    for (int i = 0; i < j; i++)
                    {
                        Console.WriteLine(i);
                    }
                    await Task.Delay(TimeSpan.FromSeconds(5), token);

                }
            });
            return Ok("In progress..");
        }
    }
}
