using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class Platform
    {
        public Platform()
        {
            Orgs = new List<Organization>();
            Bunos = new List<Buno>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Organization> Orgs { get; set; }
        public List<Buno> Bunos { get; set; }
    }

    public class PlatformConfig : IEntityTypeConfiguration<Platform>
    {
        public void Configure(EntityTypeBuilder<Platform> builder)
        {
            builder.ToTable("Platforms");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);
        }
    }
}
