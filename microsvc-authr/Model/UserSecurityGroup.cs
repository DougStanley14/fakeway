using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class UserSecurityGroup
    {
        public int NddsUserId { get; set; }
        public virtual NddsUser User { get; set; }
        public int SecurityGroupId { get; set; }
        public virtual SecurityGroup SecurityGroup { get; set; }
    }

    public class UserSecurityGroupConfig : IEntityTypeConfiguration<UserSecurityGroup>
    {
        public void Configure(EntityTypeBuilder<UserSecurityGroup> builder)
        {
            builder.ToTable("UserSecurityGroups");
            builder.HasKey(ur => new { ur.NddsUserId, ur.SecurityGroupId });

            builder.HasOne(ur => ur.User)
                    .WithMany(r => r.SecurityGroups)
                    .HasForeignKey(ur => ur.NddsUserId);

            //builder.HasOne(r => r.SecurityGroup)
            //       .WithMany(x => x.SecurityGroupUsers)
            //       .HasForeignKey(x => x.SecurityGroupId);

            //builder.HasOne(ur => ur.SecurityGroup)
            //        .WithMany(ur => ur.Users)
            //        .HasForeignKey(ur => ur.SecurityGroupId);

        }
    }
}
