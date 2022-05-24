using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class Platform
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LongName { get; set; }
        public virtual ICollection<NddsOrgPlatform> PlatformOrgs { get; set; }
        public List<Buno> Bunos { get; set; }
    }

    public class PlatformConfig : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.ToTable("Platforms");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);

            //builder.HasOne(d => d.SecurityOrgGroup)
            //           .WithMany(p => p.TMSs)
            //           .HasForeignKey(d => d.NddsOrgId)
            //           .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
