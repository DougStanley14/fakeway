using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using microsvc_authr.Data;
using microsvc_authr.Model;

namespace microsvc_authr.Services
{
    public interface IUserProfileService
    {
        List<NddsUser> AllUsers();
        Task<NddsUser> GetUser(long edipi);
        List<NddsOrg> AllSecurityGroups();
        List<String> AllSecurityGroupNames();
    }

    public class UserProfileService : IUserProfileService
    {
        private readonly NddsAuthRContext _db;
        private readonly ILogger<UserProfileService> _lgr;

        public UserProfileService(NddsAuthRContext context, ILogger<UserProfileService> logger)
        {
            _db = context;
            _lgr = logger;
        }

        public List<string> AllSecurityGroupNames()
        {
            var names = _db.NddsOrgs
                           .GroupBy( s => s.Name)
                           .Select(g => g.Key)
                           .ToList();

            return names;
        }

        public List<NddsOrg> AllSecurityGroups()
        {
            var secGrps = _db.NddsOrgs.ToList();

            return secGrps;
        }

        public List<NddsUser> AllUsers()
        {
            var users = _db.Users.ToList();

            return users;
        }

        public async Task<NddsUser> GetUser(long edipi)
        {
            var usr = await _db.Users
                               .Include(u => u.NddsOrgs)
                                  .ThenInclude(s => s.OrgGroup)
                                    //.ThenInclude(sg => sg.TMSs)
                                        //.ThenInclude(t => t.Bunos)
                               .Where(u => u.EDIPI == edipi)
                               .SingleOrDefaultAsync();

            return usr;
        }
    }
}
