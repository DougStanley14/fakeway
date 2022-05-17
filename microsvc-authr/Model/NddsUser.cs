using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class NddsUser
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public long EDIPI { get; set; }

        [MaxLength(20)]
        public string UserName { get; set; }

        public bool IsProducer { get; set; }

        public bool IsConsumer { get; set; }

        //[MaxLength(50)]
        //public string FirstName { get; set; }

        //[MaxLength(50)]
        //public string LastName { get; set; }

        //[MaxLength(50)]
        //public string FullName { get; set; }

        //[MaxLength(80)]
        //public string Email { get; set; }

        public virtual ICollection<UserSecurityGroup> SecurityGroups { get; set; }
    }

    public class NddsUserConfig : IEntityTypeConfiguration<NddsUser>
    {
        public void Configure(EntityTypeBuilder<NddsUser> builder)
        {
            builder.ToTable("NddsUsers");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.EDIPI);

        }
    }
}
