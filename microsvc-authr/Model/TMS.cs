using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class TMS
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public int SecurityOrgGroupId { get; set; }
        public virtual SecurityOrgGroup SecurityOrgGroup { get; set; }
        public List<Buno> Bunos { get; set; }
    }

    public class TMSConfig : IEntityTypeConfiguration<TMS>
    {
        public void Configure(EntityTypeBuilder<TMS> builder)
        {
            builder.ToTable("TMSs");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);

            builder.HasOne(d => d.SecurityOrgGroup)
                       .WithMany(p => p.TMSs)
                       .HasForeignKey(d => d.SecurityOrgGroupId)
                       .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
