﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace microsvc_authr.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly ILogger<AuthorizationController> _lgr;
        private readonly ITokenBuilder _tokenBuilder;

        public AuthorizationController(ILogger<AuthorizationController> logger, ITokenBuilder tokenBuilder)
        {
            _lgr = logger;
            _tokenBuilder = tokenBuilder;
        }

        [HttpGet("authorize")]
        public async Task<IActionResult> Authorize()
        {
            _lgr.LogDebug($"You are authenticated as : {User.FindFirst("preferred_username")?.Value}");

            var claims = User.Claims;

            var edipi = claims.Where(c => c.Type == "edipi").Select(x => x.Value).FirstOrDefault();

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

            var token = _tokenBuilder.BuildToken(edipi);

            return Ok(token);
        }
    }
}
