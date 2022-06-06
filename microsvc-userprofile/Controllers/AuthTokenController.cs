using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using microsvc_userprofile.DTO;
using microsvc_userprofile.Services;
using System.Security.Claims;

namespace microsvc_userprofile.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthTokenController : ControllerBase
    {
        private readonly ILogger<AuthTokenController> _lgr;
        private readonly IUserProfileService _usvc;
        private readonly IConfiguration _config;
        private readonly ClaimsPrincipal _user;

        public AuthTokenController(IUserProfileService userSvc,
                                   ILogger<AuthTokenController> logger,
                                   IHttpContextAccessor httpCxtAcc)
        {
            _lgr = logger;
            _usvc = userSvc;
            _user = httpCxtAcc.HttpContext.User;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<UserClamisMeta>> Get()
        {
            var token = await _usvc.GenerateAuthRToken();

            return Ok(token);
        }
    }
}
