using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microsvc_Mega.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MegaController : ControllerBase
    {
        private readonly ILogger<MegaController> _lgr;

        public MegaController(ILogger<MegaController> logger)
        {
            _lgr = logger;
        }

        [HttpGet]
        public IEnumerable<FileStuff> Get()
        {
            _lgr.LogInformation("Mega Getting File Stuff");
            return new List<FileStuff>
            {
                new FileStuff {FileName ="File1.doc",        Size=8983983},
                new FileStuff {FileName ="Another File.doc", Size=444},
                new FileStuff {FileName ="Third File.doc",   Size=8983983},
                new FileStuff {FileName ="File4.doc",        Size=8983983},
            };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Mega Get: {id}";
        }
        [HttpPost]
        public string Post([FromForm] string value)
        {
            return $"Mega Post:{value}";
        }
        [HttpPut("{id}")]
        public string Put(int id, [FromForm] string value)
        {
            return $"Mega Put:{id}:{value}";
        }
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"Mega Delete:{id}";
        }
    }
}
