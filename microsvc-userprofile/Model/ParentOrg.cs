using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_userprofile.Model
{
    public class ParentOrg
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public ParentOrgType ParentOrgType { get; set; }
        public string Name { get; set; }
        public string LongName { get; set; }
        public IEnumerable<Organization> Orgs { get; set; }
    }

    public class ParentOrgConfig : IEntityTypeConfiguration<ParentOrg>
    {
        public void Configure(EntityTypeBuilder<ParentOrg> builder)
        {
            builder.ToTable("ParentOrgs");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.LongName);

            builder.Property(b => b.ParentOrgType)
                   .HasConversion<string>();
        }
    }
    public enum ParentOrgType
    {
        WingMaw,
        OtherSuperGroup,
    }
}
