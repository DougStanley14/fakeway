using Microsoft.EntityFrameworkCore;
using microsvc_userprofile.Data.Seeders;
using microsvc_userprofile.Model;

namespace microsvc_userprofile.Data
{
    public static class AuthSeeders
    {
        public static void SeedData(this ModelBuilder mb)
        {
            StaticSeeds(mb);
        }

        private static void StaticSeeds(ModelBuilder mb)
        {
            mb.Entity<User>()
               .HasData(LookupSeeds.NddsUsers());

            //mb.Entity<UserSecurityGroup>()
            //   .HasData(LookupSeeds.UserSecurityGroups());

            //mb.Entity<SecurityGroup>()
            //   .HasData(LookupSeeds.SecurityGrops());
        }
    }
}
