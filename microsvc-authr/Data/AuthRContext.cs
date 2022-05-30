using Microsoft.EntityFrameworkCore;
using microsvc_authr.Model;

namespace microsvc_authr.Data
{
    public class AuthRContext : DbContext
    {
        public AuthRContext()
        {
        }

        public AuthRContext(DbContextOptions<AuthRContext> options)
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

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<ParentOrg> ParentOrgs { get; set; }
        public virtual DbSet<Organization> Organizations { get; set; }
        public virtual DbSet<Platform> Platforms { get; set; }
        public virtual DbSet<NddsProgram> Programs { get; set; }
        public virtual DbSet<Buno> Bunos { get; set; }
        public virtual DbSet<Tag> Tags { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {
            //mb.ApplyConfiguration(new SecurityGroupFlatDumpConfig());

            mb.ApplyConfiguration(new ParentOrgConfig());
            mb.ApplyConfiguration(new OrganizationConfig());
            mb.ApplyConfiguration(new PlatformConfig());
            mb.ApplyConfiguration(new BunoConfig());
            mb.ApplyConfiguration(new UserConfig());
            mb.ApplyConfiguration(new NddsProgramConfig());
            mb.ApplyConfiguration(new TagConfig());

            // mb.SeedData(); // Turn on or off depending on need
        }
    }
}