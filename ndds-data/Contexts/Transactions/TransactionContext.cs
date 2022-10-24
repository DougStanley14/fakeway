using Microsoft.EntityFrameworkCore;
using ndds.model.Transactions;

namespace ndds.data.Contexts.Transactions
{
    public class TransactionContext : DbContext
    {
        public TransactionContext()
        {
        }

        public TransactionContext(DbContextOptions<TransactionContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.EnableSensitiveDataLogging();

                optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=NDDSTransactions;Integrated Security=True");      // Local SQL
                //optionsBuilder.UseSqlServer("Data Source=PDSQLT01.apps.navair1.navy.mil\tst01;Initial Catalog=WD_IBOS;Integrated Security=True"); // Test SQL
            }
        }

        public virtual DbSet<Operation> Operations { get; set; }


        protected override void OnModelCreating(ModelBuilder mb)
        {
            //mb.ApplyConfiguration(new SecurityGroupFlatDumpConfig());

            mb.ApplyConfiguration(new OperationConfig());


            // mb.SeedData(); // Turn on or off depending on need
        }
    }
}