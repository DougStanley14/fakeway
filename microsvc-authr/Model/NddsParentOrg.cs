using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class NddsParentOrg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public NddsParentOrgType ParentOrgType { get; set; }
        public string Name { get; set; }
        public string LongName { get; set; }
        public IEnumerable<NddsOrg> NddsOrgs { get; set; }
    }

    public class NddsParentOrgConfig : IEntityTypeConfiguration<NddsParentOrg>
    {
        public void Configure(EntityTypeBuilder<NddsParentOrg> builder)
        {
            builder.ToTable("NddsParentOrgs");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.LongName);

            builder.Property(b => b.ParentOrgType)
                   .HasConversion<string>();
        }
    }
    public enum NddsParentOrgType
    {
        WingMaw,
        OtherSuperGroup,
    }
}
