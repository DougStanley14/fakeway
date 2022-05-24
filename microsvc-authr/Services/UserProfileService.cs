using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using microsvc_authr.Data;
using microsvc_authr.Model;

namespace microsvc_authr.Services
{
    public interface IUserProfileService
    {
        List<User> AllUsers();
        Task<User> GetUser(long edipi);
        List<Organization> AllSecurityGroups();
        List<String> AllSecurityGroupNames();
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
                           .GroupBy( s => s.Name)
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

        public async Task<User> GetUser(long edipi)
        {
            var usr = await _db.Users
                               .Include(u => u.UserOrgs)
                                  .ThenInclude(s => s.OrgGroup)
                                    //.ThenInclude(sg => sg.TMSs)
                                        //.ThenInclude(t => t.Bunos)
                               .Where(u => u.EDIPI == edipi)
                               .SingleOrDefaultAsync();

            return usr;
        }
    }
}
