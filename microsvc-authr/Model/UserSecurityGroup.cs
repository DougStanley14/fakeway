using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class UserSecurityGroup
    {
        public int NddsUserId { get; set; }
        public virtual NddsUser User { get; set; }
        public int SecurityGroupId { get; set; }
        public virtual SecurityOrgGroup SecurityGroup { get; set; }
    }

    public class UserSecurityGroupConfig : IEntityTypeConfiguration<UserSecurityGroup>
    {
        public void Configure(EntityTypeBuilder<UserSecurityGroup> builder)
        {
            builder.ToTable("UserSecurityGroups");
            builder.HasKey(ur => new { ur.NddsUserId, ur.SecurityGroupId });

            builder.HasOne(a => a.User)
                   .WithMany(b => b.SecurityGroups)
                   .HasForeignKey(c => c.NddsUserId);

            builder.HasOne(a => a.SecurityGroup)
                   .WithMany(b => b.SecurityGroupUsers)
                   .HasForeignKey(c => c.SecurityGroupId);  

        }
    }
}
