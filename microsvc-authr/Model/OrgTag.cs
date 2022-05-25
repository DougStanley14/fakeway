using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class OrgTag
    {
        public int OrganizationId { get; set; }
        public virtual Organization Org { get; set; }
        public int TagId { get; set; }
        public virtual Tag Tag { get; set; }
    }

    public class OrgTagConfig : IEntityTypeConfiguration<OrgTag>
    {
        public void Configure(EntityTypeBuilder<OrgTag> builder)
        {
            builder.ToTable("OrgTags");
            builder.HasKey(ur => new { ur.OrganizationId, ur.TagId });

            builder.HasOne(a => a.Org)
                   .WithMany(b => b.OrgTags)
                   .HasForeignKey(c => c.OrganizationId);

            builder.HasOne(a => a.Tag)
                   .WithMany(b => b.TagOrgs)
                   .HasForeignKey(c => c.TagId);  

        }
    }
}
