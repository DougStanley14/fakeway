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
        List<SecurityOrgGroup> AllSecurityGroups();
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
            var names = _db.SecurityOrgGroups
                           .GroupBy( s => s.Name)
                           .Select(g => g.Key)
                           .ToList();

            return names;
        }

        public List<SecurityOrgGroup> AllSecurityGroups()
        {
            var secGrps = _db.SecurityOrgGroups.ToList();

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
                               .Include(u => u.SecurityGroups)
                                  .ThenInclude(s => s.SecurityGroup)
                                    .ThenInclude(sg => sg.TMSs)
                                        .ThenInclude(t => t.Bunos)
                               .Where(u => u.EDIPI == edipi)
                               .SingleOrDefaultAsync();

            return usr;
        }
    }
}
