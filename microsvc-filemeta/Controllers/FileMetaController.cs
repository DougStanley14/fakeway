using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microsvc_filemeta.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileMetaController : ControllerBase
    {
        private readonly ILogger<FileMetaController> _lgr;

        public FileMetaController(ILogger<FileMetaController> logger)
        {
            _lgr = logger;
        }

        [Authorize(Roles = "Producer")]
        [Authorize(Policy = "UserTest1")]
        [HttpGet]
        public IEnumerable<FileStuff> Get()
        {
            _lgr.LogInformation("Getting File Stuff");
            return new List<FileStuff>
            {
                new FileStuff {FileName ="File1.doc",        Size=8983983},
                new FileStuff {FileName ="Another File.doc", Size=444},
                new FileStuff {FileName ="Third File.doc",   Size=8983983},
                new FileStuff {FileName ="File4.doc",        Size=8983983},
            };
        }

        [Authorize(Policy = "Consumer")]
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return $"Get: {id}";
        }

        [Authorize(Policy = "Producer")]
        [HttpPost]
        public string Post([FromForm] string value)
        {
            return $"Post:{value}";
        }

        [HttpPut("{id}")]
        public string Put(int id, [FromForm] string value)
        {
            return $"Put:{id}:{value}";
        }

        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            return $"Delete:{id}";
        }
    }


}
public class FileStuff
{
    public string FileName { get; set; }
    public int Size { get; set; }
}