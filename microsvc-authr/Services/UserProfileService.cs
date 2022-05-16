using Microsoft.AspNetCore.Mvc;
using microsvc_authr.Data;
using microsvc_authr.Model;

namespace microsvc_authr.Services
{
    public interface IUserProfileService
    {
        List<NddsUser> AllUsers();
        NddsUser GetUser(long edipi);
        List<SecurityGroup> AllSecurityGroups();
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
            var names = _db.SecurityGroups
                           .GroupBy( s => s.OrgCode)
                           .Select(g => g.Key)
                           .ToList();

            return names;
        }

        public List<SecurityGroup> AllSecurityGroups()
        {
            var secGrps = _db.SecurityGroups.ToList();

            return secGrps;
        }

        public List<NddsUser> AllUsers()
        {
            var users = _db.Users.ToList();

            return users;
        }

        public NddsUser GetUser(long edipi)
        {
            var usr = _db.Users.FirstOrDefault(u => u.EDIPI == edipi);

            return usr;
        }
    }
}
