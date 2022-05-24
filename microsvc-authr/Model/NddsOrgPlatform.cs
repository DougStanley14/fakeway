using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class NddsOrgPlatform
    {
        public int NddsOrgId { get; set; }
        public virtual NddsOrg NddsOrg { get; set; }
        public int PlatformId { get; set; }
        public virtual Platform Platform { get; set; }
    }

    public class NddsOrgPlatformConfig : IEntityTypeConfiguration<NddsOrgPlatform>
    {
        public void Configure(EntityTypeBuilder<NddsOrgPlatform> builder)
        {
            builder.ToTable("NddsOrgPlatforms");
            builder.HasKey(ur => new { ur.NddsOrgId, ur.PlatformId });

            builder.HasOne(a => a.NddsOrg)
                   .WithMany(b => b.OrgPlatforms)
                   .HasForeignKey(c => c.NddsOrgId);

            builder.HasOne(a => a.Platform)
                   .WithMany(b => b.PlatformOrgs)
                   .HasForeignKey(c => c.PlatformId);  

        }
    }
}
