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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();

                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NDDSMeta;Integrated Security=True");      // Local SQL
                //optionsBuilder.UseSqlServer("Data Source=PDSQLT01.apps.navair1.navy.mil\tst01;Initial Catalog=WD_IBOS;Integrated Security=True"); // Test SQL
            }
        }

        public virtual DbSet<NddsUser> Users { get; set; }
        public virtual DbSet<UserNddsOrg> UserNddsOrgs { get; set; }
        public virtual DbSet<NddsParentOrg> NddsParentOrgs { get; set; }
        public virtual DbSet<NddsOrg> NddsOrgs { get; set; }
        public virtual DbSet<NddsOrgPlatform> NddsOrgPlatforms { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<NddsOrgSysProg> NddsOrgSysProgs { get; set; }
        public virtual DbSet<SysProg> SysProgs { get; set; }
        public virtual DbSet<Buno> Bunos { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //mb.ApplyConfiguration(new SecurityGroupFlatDumpConfig());

            mb.ApplyConfiguration(new NddsParentOrgConfig());
            mb.ApplyConfiguration(new SecurityOrgGroupConfig());
            mb.ApplyConfiguration(new PlatformConfig());
            mb.ApplyConfiguration(new BunoConfig());
            mb.ApplyConfiguration(new NddsUserConfig());
            mb.ApplyConfiguration(new UserConsumerOrgGroupConfig());
            mb.ApplyConfiguration(new NddsOrgPlatformConfig());
            mb.ApplyConfiguration(new SysProgConfig());
            mb.ApplyConfiguration(new NddsOrgSysProgConfig());

            mb.SeedData();
        }
    }
}