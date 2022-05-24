using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class OrgPlatform
    {
        public int OrganizationId { get; set; }
        public virtual Organization Org { get; set; }
        public int PlatformId { get; set; }
        public virtual Platform Platform { get; set; }
    }

    public class OrgPlatformConfig : IEntityTypeConfiguration<OrgPlatform>
    {
        public void Configure(EntityTypeBuilder<OrgPlatform> builder)
        {
            builder.ToTable("OrgPlatforms");
            builder.HasKey(ur => new { ur.OrganizationId, ur.PlatformId });

            builder.HasOne(a => a.Org)
                   .WithMany(b => b.OrgPlatforms)
                   .HasForeignKey(c => c.OrganizationId);

            builder.HasOne(a => a.Platform)
                   .WithMany(b => b.PlatformOrgs)
                   .HasForeignKey(c => c.PlatformId);  

        }
    }
}
