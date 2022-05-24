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
            };

            if (user.Producer) claims.Add(new Claim(ClaimTypes.Role, "Producer"));
            if (user.Consumer) claims.Add(new Claim(ClaimTypes.Role, "Consumer"));


            var prof = new
            {
                Name = user.UserName, 
                Edipi = user.EDIPI,
                Grps = user.UserOrgs.Select(g => new
                {
                    g.OrgGroup.LongName,
                    g.OrgGroup.Name,
                    //TMSs = g.OrgGroup.TMSs.Select(t => new
                    //{
                    //    t.Name
                    //}).ToList()
                }).ToList()
            };

            _lgr.LogInformation("User Profile {@prof}", prof);


            foreach (var sg in user.UserOrgs)
            {
                var tms = sg.OrgGroup
                            .OrgPlatforms
                            .GroupBy( t => t.Platform.Name)
                            .Select( g => g.Key)
                            .ToList();

                var sgname = sg.OrgGroup.Name;

                tms.ForEach(t => claims.Add(new Claim("Platform", $"{sgname}:{t}")));

            }

            //// TMS
            //user.SecurityGroups.GroupBy(s => s.SecurityGroup.Tms)
            //                   .Select(g => g.Key).ToList()
            //                   .ForEach(x => claims.Add(new Claim("TMS", x)));

            //// TypeModel
            //user.SecurityGroups.GroupBy(s => s.SecurityGroup.TypeModel)
            //                   .Select(g => g.Key).ToList()
            //                   .ForEach(x => claims.Add(new Claim("TypeModel", x)));

            //// Org
            //user.SecurityGroups.GroupBy(s => s.SecurityGroup.Org)
            //                   .Select(g => g.Key).ToList()
            //                   .ForEach(x => claims.Add(new Claim("Org", x)));

            //// OrgCode
            //user.SecurityGroups.GroupBy(s => s.SecurityGroup.OrgCode)
            //                   .Select(g => g.Key).ToList()
            //                   .ForEach(x => claims.Add(new Claim("OrgCode", x)));

            //// Buno
            //user.SecurityGroups.GroupBy(s => s.SecurityGroup.Buno)
            //                   .Select(g => g.Key).ToList()
            //                   .ForEach(x => claims.Add(new Claim("Buno", x.ToString())));


            return claims;

        }
    }
}
