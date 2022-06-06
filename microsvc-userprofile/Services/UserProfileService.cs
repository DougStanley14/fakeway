using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using microsvc_userprofile.Data;
using microsvc_userprofile.DTO;
using microsvc_userprofile.Model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace microsvc_userprofile.Services
{
    public interface IUserProfileService
    {
        List<string> AllSecurityGroupNames();
        List<Organization> AllSecurityGroups();
        List<User> AllUsers();
        Task<string> GenerateAuthRToken();
        Task<UserClamisMeta> GetUserMeta(long edipi);
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly AuthRContext _db;
        private readonly string _tokenSecret;
        private readonly ILogger<UserProfileService> _lgr;
        private readonly ClaimsPrincipal _authUser;

        public UserProfileService(AuthRContext context,
                                  IConfiguration config,
                                  IHttpContextAccessor httpCxtAcc,
                                  ILogger<UserProfileService> logger)
        {
            _db = context;
            _lgr = logger;
            _tokenSecret = config["Jwt:TwoGuidSecret"];
            _authUser = httpCxtAcc.HttpContext.User;
        }

        public List<string> AllSecurityGroupNames()
        {
            var names = _db.Organizations
                           .GroupBy(s => s.Code)
                           .Select(g => g.Key)
                           .ToList();

            return names;
        }

        public List<Organization> AllSecurityGroups()
        {
            var secGrps = _db.Organizations.ToList();

            return secGrps;
        }

        public List<User> AllUsers()
        {
            var users = _db.Users.ToList();

            return users;
        }

        public async Task<UserClamisMeta> GetUserMeta(long edipi)
        {
            var users = _db.Users.ToList();

            var usr = await _db.Users.Where(u => u.EDIPI == edipi)
                                      .Select(u => new UserClamisMeta
                                      {
                                          EDIPI = u.EDIPI,
                                          UserName = u.UserName,
                                          OrgClaimsMeta = u.Orgs.Select(o => new UserOrgsClaimsMeta
                                          {
                                              OrgName = o.Code,
                                              OrgType = o.OrgType,
                                              Platforms = o.Platforms.Select(p => p.Name).ToList(),
                                              Programs = o.Programs.Select(p => p.Name).ToList(),
                                              Bunos = o.Platforms.SelectMany(p => p.Bunos.Select(b => b.BunoCode)).ToList()
                                          }).ToList()
                                      }).FirstOrDefaultAsync();

            return usr;
        }

        public async Task<string> GenerateAuthRToken()
        {
            var edipi = _authUser.Claims
                 .Where(c => c.Type == "edipi")              // From AuthN
                 .Select(x => x.Value).FirstOrDefault();

            var usermeta = await GetUserMeta(long.Parse(edipi));

            var token = BuildToken(usermeta);

            return token;
        }


        //TODO:  Refactor to Shared Library
        private string BuildToken(UserClamisMeta usermeta)
        {
            _lgr.LogInformation("Building Token for {@meta}", usermeta.UserName);

            var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenSecret));
            var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

            var claims = GetUserClaims(usermeta);

            var jwt = new JwtSecurityToken(issuer: "NDDS",
                                           audience: "NDDS",
                                           claims: claims,
                                           expires: DateTime.Now.AddDays(5),
                                           signingCredentials: signingCredentials);

            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);

            return encodedJwt;
        }

        private List<Claim> GetUserClaims(UserClamisMeta usermeta)
        {
            var claims = new List<Claim> {
                new Claim("edipi", usermeta.EDIPI.ToString()),
                new Claim("username", usermeta.UserName ),
            };

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
