using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class Organization
    {
        public Organization()
        {
            Tags = new List<Tag>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public OrgType OrgType { get; set; }
        public string Name { get; set; }
        public string? LongName { get; set; }
        public int? ParentOrgId { get; set; }
        public virtual ParentOrg? ParentOrg { get; set; }
        public virtual ICollection<OrgPlatform> OrgPlatforms { get; set; }
        public virtual ICollection<NddsProgram> Programs { get; set; }
        public virtual ICollection<UserOrg> OrgUsers { get; set; }
        public virtual ICollection<Tag> Tags { get; set; }
    }

    public class OrganizationConfig : IEntityTypeConfiguration<Organization>
    {
        public void Configure(EntityTypeBuilder<Organization> builder)
        {
            builder.ToTable("Organizations");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.LongName);

            builder.HasOne(d => d.ParentOrg)
                       .WithMany(p => p.Orgs)
                       .HasForeignKey(d => d.ParentOrgId)
                       .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Property(b => b.OrgType)
                   .HasConversion<string>();
        }
    }

    public enum OrgType
    {
        Producer,
        Consumer,
    }
}
