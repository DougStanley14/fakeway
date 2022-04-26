using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microsvc_filemgr.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FileStoreController : ControllerBase
    {
        private readonly ILogger<FileStoreController> _lgr;

        public FileStoreController(ILogger<FileStoreController> logger)
        {
            _lgr = logger;
        }

    }
}
