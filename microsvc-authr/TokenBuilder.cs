using Microsoft.IdentityModel.Tokens;
using microsvc_authr.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace microsvc_authr
{
    public interface ITokenBuilder
    {
        Task<string> BuildToken(string sskSecret, string userName, string edipi);
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

        public async Task<string> BuildToken(string sskSecret, string userName, string edipi)
        {
            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(sskSecret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = await GetUserClaims(userName, edipi);
            
            var jwt = new JwtSecurityToken(issuer:"SydTestAuthR", 
                                           audience: "NDDS", 
                                           claims: claims, 
                                           expires: DateTime.Now.AddDays(5),
                                           signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private async Task<List<Claim>> GetUserClaims(string userName, string edipi)
        {
            var edipilong = long.Parse(edipi);
            var usermeta = await _usvc.GetUserMeta(edipilong);

            var claims = new List<Claim> {
                new Claim("EDIPI", edipi),
                new Claim("preferred_username", userName ),
            };

            _lgr.LogInformation("User Profile {@meta}", usermeta);

            // Add Producer and/or Consumer Claim - Just one of each
            var roles = usermeta.OrgClaimsMeta.GroupBy(o => o.OrgType)
                                  .Select(g => g.Key)
                                  .ToList();

            if (roles.Any(r => r == Model.OrgType.Producer)) claims.Add(new Claim(ClaimTypes.Role, "Producer"));
            if (roles.Any(r => r == Model.OrgType.Consumer)) claims.Add(new Claim(ClaimTypes.Role, "Consumer"));

            foreach (var org in usermeta.OrgClaimsMeta)
            {

                org.Platforms.ForEach(p => claims.Add(new Claim("Platform", $"{org.OrgName}:{p}")));
                org.Programs.ForEach(p => claims.Add(new Claim("Program", $"{org.OrgName}:{p}")));

            }


            return claims;

        }
    }
}
