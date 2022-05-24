using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class Buno
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int BunoCode { get; set; }
        public string Location { get; set; }
        public DateTime QtrEndDate { get; set; }
        public int PlatformId { get; set; }
        public virtual Platform Platform { get; set; }
    }
    public class BunoConfig : IEntityTypeConfiguration<Buno>
    {
        public void Configure(EntityTypeBuilder<Buno> builder)
        {
            builder.ToTable("Bunos");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.BunoCode);
            builder.HasIndex(e => e.QtrEndDate);

            builder.HasOne(d => d.Platform)
                       .WithMany(p => p.Bunos)
                       .HasForeignKey(d => d.PlatformId)
                       .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
