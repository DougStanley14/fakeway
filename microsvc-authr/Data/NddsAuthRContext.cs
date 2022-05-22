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
        //public virtual DbSet<SecurityGroupFlatDump> SecurityGroupFlatDumps { get; set; }
        public virtual DbSet<UserSecurityGroup> UserSecurityGroups { get; set; }
        public virtual DbSet<WingMaw> WingMaws { get; set; }
        public virtual DbSet<SecurityOrgGroup> SecurityOrgGroups { get; set; }
        public virtual DbSet<TMS> TMSs { get; set; }
        public virtual DbSet<Buno> Bunos { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //mb.ApplyConfiguration(new SecurityGroupFlatDumpConfig());

            mb.ApplyConfiguration(new WingMawConfig());
            mb.ApplyConfiguration(new SecurityOrgGroupConfig());
            mb.ApplyConfiguration(new TMSConfig());
            mb.ApplyConfiguration(new BunoConfig());
            mb.ApplyConfiguration(new NddsUserConfig());
            mb.ApplyConfiguration(new UserSecurityGroupConfig());

            mb.SeedData();
        }
    }
}