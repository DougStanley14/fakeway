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
    public class UserProfileController : ControllerBase
    {
        private readonly ILogger<UserProfileController> _lgr;
        private readonly IUserProfileService _usvc;
        private readonly IConfiguration _config;
        private readonly ClaimsPrincipal _user;

        public UserProfileController(IUserProfileService userSvc,
                                   ILogger<UserProfileController> logger,
                                   IHttpContextAccessor httpCxtAcc)
        {
            _lgr = logger;
            _usvc = userSvc;
            _user = httpCxtAcc.HttpContext.User;
        }

        [Authorize]
        [HttpGet("{edipi}")]
        public async Task<ActionResult<UserClamisMeta>> Get(long edipi)
        {
            var uprof = await _usvc.GetUserMeta(edipi);

            return Ok(uprof);
        }
    }
}
