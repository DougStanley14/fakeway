using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class NddsOrg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public NddsOrgType OrgType { get; set; }
        public string Name { get; set; }
        public string LongName { get; set; }
        public int NddsParentOrgId { get; set; }
        public virtual NddsParentOrg ParentOrg { get; set; }
        public virtual ICollection<NddsOrgPlatform> OrgPlatforms { get; set; }
        public virtual ICollection<NddsOrgSysProg> OrgPrograms { get; set; }

        public virtual ICollection<UserNddsOrg> OrgUsers { get; set; }
    }

    public class SecurityOrgGroupConfig : IEntityTypeConfiguration<NddsOrg>
    {
        public void Configure(EntityTypeBuilder<NddsOrg> builder)
        {
            builder.ToTable("NddsOrg");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.LongName);

            builder.HasOne(d => d.ParentOrg)
                       .WithMany(p => p.NddsOrgs)
                       .HasForeignKey(d => d.NddsParentOrgId)
                       .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }

    public enum NddsOrgType
    {
        Producer,
        Consumer,
    }
}
