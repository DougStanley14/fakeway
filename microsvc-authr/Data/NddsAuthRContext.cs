using Microsoft.EntityFrameworkCore;
using microsvc_authr.Model;

namespace microsvc_authr.Data
{
    public class NddsAuthRContext : DbContext
    {
        public NddsAuthRContext()
        {
        }

        public NddsAuthRContext(DbContextOptions<NddsAuthRContext> options)
            : base(options)
        {
        }

        public virtual DbSet<NddsUser> Users { get; set; }
        public virtual DbSet<SecurityGroup> SecurityGroups { get; set; }
        public virtual DbSet<UserSecurityGroup> UserSecurityGroups { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {

            mb.ApplyConfiguration(new NddsUserConfig());
            mb.ApplyConfiguration(new SecurityGroupConfig());
            mb.ApplyConfiguration(new UserSecurityGroupConfig());

            mb.SeedData();
        }
    }
}