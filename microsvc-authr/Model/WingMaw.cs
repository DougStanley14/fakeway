using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class WingMaw
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string WingMawCode { get; set; }
        public IEnumerable<SecurityOrgGroup> SecGroups { get; set; }
    }

    public class WingMawConfig : IEntityTypeConfiguration<WingMaw>
    {
        public void Configure(EntityTypeBuilder<WingMaw> builder)
        {
            builder.ToTable("WingMaws");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.WingMawCode);



        }
    }
}
