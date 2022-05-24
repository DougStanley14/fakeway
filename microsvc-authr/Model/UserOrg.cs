using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class UserOrg
    {
        public int UserId { get; set; }
        public virtual User User { get; set; }
        public int OrgId { get; set; }
        public virtual Organization OrgGroup { get; set; }
    }

    public class UserConsumerOrgGroupConfig : IEntityTypeConfiguration<UserOrg>
    {
        public void Configure(EntityTypeBuilder<UserOrg> builder)
        {
            builder.ToTable("UserOrgs");
            builder.HasKey(ur => new { ur.UserId, ur.OrgId });

            builder.HasOne(a => a.User)
                   .WithMany(b => b.UserOrgs)
                   .HasForeignKey(c => c.UserId);

            builder.HasOne(a => a.OrgGroup)
                   .WithMany(b => b.OrgUsers)
                   .HasForeignKey(c => c.OrgId);  

        }
    }
}
