using Microsoft.AspNetCore.Mvc;
using microsvc_userprofile.Services;
using System.Security.Claims;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace microsvc_userprofile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StuffController : ControllerBase
    {
        private readonly ILogger<StuffController> _lgr;
        private readonly IUserProfileService _usvc;
        private readonly IConfiguration _config;
        private readonly ClaimsPrincipal _user;
        private readonly HttpContext _ctx;

        public StuffController(IUserProfileService userSvc,
                               ILogger<StuffController> logger,
                               IHttpContextAccessor httpCxtAcc)
        {
            _lgr = logger;
            _usvc = userSvc;
            _user = httpCxtAcc.HttpContext.User;
            _ctx = httpCxtAcc.HttpContext;
        }

        // GET: api/<StuffController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<StuffController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<StuffController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<StuffController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<StuffController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}


