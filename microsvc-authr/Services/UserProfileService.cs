using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using microsvc_authr.Data;
using microsvc_authr.Model;

namespace microsvc_authr.Services
{
    public interface IUserProfileService
    {
        List<string> AllSecurityGroupNames();
        List<Organization> AllSecurityGroups();
        List<User> AllUsers();
        Task<UserClamisMeta> GetUserMeta(long edipi);
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly AuthRContext _db;
        private readonly ILogger<UserProfileService> _lgr;

        public UserProfileService(AuthRContext context, ILogger<UserProfileService> logger)
        {
            _db = context;
            _lgr = logger;
        }

        public List<string> AllSecurityGroupNames()
        {
            var names = _db.Organizations
                           .GroupBy(s => s.Name)
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
                                          OrgClaimsMeta = u.UserOrgs.Select(o => new UserOrgsClaimsMeta
                                          {
                                              OrgName = o.OrgGroup.Name,
                                              OrgType = o.OrgGroup.OrgType,
                                              Platforms = o.OrgGroup.OrgPlatforms.Select(p => p.Platform.Name).ToList(),
                                              Programs = o.OrgGroup.Programs.Select(p => p.Name).ToList(),
                                              Bunos = o.OrgGroup.OrgPlatforms.SelectMany(p => p.Platform.Bunos.Select(b => b.BunoCode)).ToList()
                                          }).ToList()
                                      }).FirstOrDefaultAsync();

            return usr;
        }
    }

    public class UserClamisMeta
    {
        public long EDIPI { get; set; }
        public string UserName { get; set; }
        public List<UserOrgsClaimsMeta> OrgClaimsMeta { get; set; }
    }

    public class UserOrgsClaimsMeta
    {
        public string OrgName { get; set; }
        public OrgType OrgType { get; set; }
        public List<string> Platforms { get; set; }
        public List<string> Programs { get; set; }
        public List<int> Bunos { get; set; }
    }
}
