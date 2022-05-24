using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class SysProg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LongName { get; set; }
        public virtual ICollection<NddsOrgSysProg> SysProgOrgs { get; set; }
    }

    public class SysProgConfig : IEntityTypeConfiguration<SysProg>
    {
        public void Configure(EntityTypeBuilder<SysProg> builder)
        {
            builder.ToTable("SysProgs");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);

            //builder.HasOne(d => d.SecurityOrgGroup)
            //           .WithMany(p => p.TMSs)
            //           .HasForeignKey(d => d.NddsOrgId)
            //           .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
