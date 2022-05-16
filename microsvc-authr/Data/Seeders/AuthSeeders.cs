using Microsoft.EntityFrameworkCore;
using microsvc_authr.Data.Seeders;
using microsvc_authr.Model;

namespace microsvc_authr.Data
{
    public static class AuthSeeders
    {
        public static void SeedData(this ModelBuilder mb)
        {
            StaticSeeds(mb);
        }

        private static void StaticSeeds(ModelBuilder mb)
        {
            mb.Entity<NddsUser>()
               .HasData(LookupSeeds.NddsUsers());

            //mb.Entity<UserSecurityGroup>()
            //   .HasData(LookupSeeds.UserSecurityGroups());

            //mb.Entity<SecurityGroup>()
            //   .HasData(LookupSeeds.SecurityGrops());
        }
    }
}
