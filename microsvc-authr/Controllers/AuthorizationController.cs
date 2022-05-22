using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using microsvc_authr.Model;
using microsvc_authr.Services;

namespace microsvc_authr.Controllers
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _lgr;
        private readonly IUserProfileService _usvc;
        private readonly ITokenBuilder _tokenBuilder;
        private readonly IConfiguration _config;

        public AuthorizationController(IUserProfileService userSvc,
                                       ILogger<AuthorizationController> logger, 
                                       ITokenBuilder tokenBuilder, 
                                       IConfiguration configuration)
        {
            _lgr = logger;
            _usvc = userSvc;
            _tokenBuilder = tokenBuilder;
            _config = configuration;
        }

        [Authorize]
        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize()
        {
            var sskeysecret = _config["Jwt:TwoGuidSecret"];
            _lgr.LogDebug($"You are authenticated as : {User.FindFirst("preferred_username")?.Value}");

            var claims = User.Claims;

            var edipi = claims.Where(c => c.Type == "edipi").Select(x => x.Value).FirstOrDefault();
            var username = User.FindFirst("preferred_username")?.Value;

            _lgr.LogDebug($"User {username} is authenticated with EDIPI : {edipi}");

            //if (dbUser == null)
            //{
            //    return NotFound("User not found.");
            //}

            // This is just an example, made for simplicity; do not store plain passwords in the database
            // Always hash and salt your passwords
            // var isValid = dbUser.Password == user.Password;

            //if (!isValid)
            //{
            //    return BadRequest("Could not authenticate user.");
            //}

            var token = _tokenBuilder.BuildToken(sskeysecret, username, edipi);

            return Ok(token);
        }


        [HttpGet("AllUsers")]
        public async Task<ActionResult<List<NddsUser>>> AllUsers()
        {
            return _usvc.AllUsers();
        }

        [HttpGet("SecurityGroups")]
        public async Task<ActionResult<List<SecurityOrgGroup>>> AllSecurityGroups()
        {
            return _usvc.AllSecurityGroups();
        }

        [HttpGet("SecurityGroupsNames")]
        public async Task<ActionResult<List<string>>> AllSecurityGroupsNames()
        {
            return _usvc.AllSecurityGroupNames();
        }

        //[HttpGet("GetUser/{edipi}")]
        //public async Task<ActionResult<NddsUser>> AllSecurityGroupsNames(long edipi)
        //{
        //    return await _usvc.GetUser(edipi);
        //}
    }
}
