using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class UserNddsOrg
    {
        public int NddsUserId { get; set; }
        public virtual NddsUser User { get; set; }
        public int NddsOrgId { get; set; }
        public virtual NddsOrg OrgGroup { get; set; }
    }

    public class UserConsumerOrgGroupConfig : IEntityTypeConfiguration<UserNddsOrg>
    {
        public void Configure(EntityTypeBuilder<UserNddsOrg> builder)
        {
            builder.ToTable("UserNddsOrgs");
            builder.HasKey(ur => new { ur.NddsUserId, ur.NddsOrgId });

            builder.HasOne(a => a.User)
                   .WithMany(b => b.NddsOrgs)
                   .HasForeignKey(c => c.NddsUserId);

            builder.HasOne(a => a.OrgGroup)
                   .WithMany(b => b.OrgUsers)
                   .HasForeignKey(c => c.NddsOrgId);  

        }
    }
}
