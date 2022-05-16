using Microsoft.IdentityModel.Tokens;
using microsvc_authr.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace microsvc_authr
{
    public interface ITokenBuilder
    {
        string BuildToken(string sskSecret, string userName, string edipi);
    }
    public class TokenBuilder : ITokenBuilder
    {
        private readonly ILogger<TokenBuilder> _lgr;
        private readonly IUserProfileService _usvc;
        public TokenBuilder(IUserProfileService userSvc,
                            ILogger<TokenBuilder> logger)
        {
            _lgr = logger;
            _usvc = userSvc;
        }

        public string BuildToken(string sskSecret, string userName, string edipi)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sskSecret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);
            var claims = new Claim[]
            {
                new Claim("SecurityGroup", "PMA Supers"),
                new Claim("Platform", "CVN"),
                new Claim("Platform", "DDG"),
                new Claim("Airwing", "CVW-2"),
                new Claim("Airwing", "CVW-9"),
                new Claim("Airwing", "TAW-4"),
                new Claim("NameyName", "Some Random Name"),
                new Claim("EDIPI", edipi),
            };

            var jwt = new JwtSecurityToken(issuer:"SydTestAuthR", 
                                           audience: "NDDS", 
                                           claims: claims, 
                                           expires: DateTime.Now.AddDays(5),
                                           signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }
    }
}
