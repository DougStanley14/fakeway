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
            var user = await _usvc.GetUser(edipilong);

            var claims = new List<Claim> {
                new Claim("EDIPI", edipi),
                new Claim("preferred_username", userName ),

                
                //new Claim("IsProducer",
                //           user.IsProducer ? bool.TrueString : bool.FalseString,
                //           ClaimValueTypes.Boolean),
                //new Claim("IsConsumer",
                //           user.IsConsumer ? bool.TrueString : bool.FalseString,
                //           ClaimValueTypes.Boolean),
            };

            if (user.IsProducer) claims.Add(new Claim(ClaimTypes.Role, "IsProducer"));
            if (user.IsConsumer) claims.Add(new Claim(ClaimTypes.Role, "IsConsumer"));


            var prof = new
            {
                Name = user.UserName, 
                Edipi = user.EDIPI,
                Grps = user.SecurityGroups.Select(g => new
                {
                    g.SecurityGroup.Org,
                    g.SecurityGroup.Buno,
                    g.SecurityGroup.WingMawCode
                })
            };

            _lgr.LogInformation("User Profile {@prof}", prof);

            // TMS
            user.SecurityGroups.GroupBy(s => s.SecurityGroup.Tms)
                               .Select(g => g.Key).ToList()
                               .ForEach(x => claims.Add(new Claim("TMS", x)));

            // TypeModel
            user.SecurityGroups.GroupBy(s => s.SecurityGroup.TypeModel)
                               .Select(g => g.Key).ToList()
                               .ForEach(x => claims.Add(new Claim("TypeModel", x)));

            // Org
            user.SecurityGroups.GroupBy(s => s.SecurityGroup.Org)
                               .Select(g => g.Key).ToList()
                               .ForEach(x => claims.Add(new Claim("Org", x)));

            // OrgCode
            user.SecurityGroups.GroupBy(s => s.SecurityGroup.OrgCode)
                               .Select(g => g.Key).ToList()
                               .ForEach(x => claims.Add(new Claim("OrgCode", x)));

            // Buno
            user.SecurityGroups.GroupBy(s => s.SecurityGroup.Buno)
                               .Select(g => g.Key).ToList()
                               .ForEach(x => claims.Add(new Claim("Buno", x.ToString())));


            return claims;

        }
    }
}
