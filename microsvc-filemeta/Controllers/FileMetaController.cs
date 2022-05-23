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
        private readonly IAuthorizationService _authSvc;

        public FileMetaController(ILogger<FileMetaController> logger,
                                  IAuthorizationService authorizationService)
        {
            _lgr = logger;
            _authSvc = authorizationService;
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

        [Authorize(Roles = "Producer")]
        [HttpPost]
        public async Task<ActionResult<NDFileMeta>> Post([FromForm] NDFileMeta fileMeta)
        {
            var authorizationResult = await _authSvc
                .AuthorizeAsync(User, fileMeta, "FileMetaTestPolicy");

            if (authorizationResult.Succeeded)
            {
                // Actually Post the Data Here
                return fileMeta;
            }
            else
            {
                return new ForbidResult();
            }

            
        }

        [HttpPut("{id}")]
        public string Put(int id, [FromForm] string FormVal1)
        {
            return $"Put:{id}:{FormVal1}";
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

public class NDFileMeta
{
    public string FileHash { get; set; }
    public string UserSecGroup { get; set; }
    public List<string> Platform { get; set; }
}