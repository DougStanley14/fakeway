using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class SecurityOrgGroup
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string GroupOrg { get; set; }
        public int WingMawId { get; set; }
        public virtual WingMaw WingMaw { get; set; }
        public List<TMS> TMSs { get; set; }
        public virtual ICollection<UserSecurityGroup> SecurityGroupUsers { get; set; }
    }

    public class SecurityOrgGroupConfig : IEntityTypeConfiguration<SecurityOrgGroup>
    {
        public void Configure(EntityTypeBuilder<SecurityOrgGroup> builder)
        {
            builder.ToTable("SecurityOrgGroups");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);
            builder.HasIndex(e => e.GroupOrg);

            builder.HasOne(d => d.WingMaw)
                       .WithMany(p => p.SecGroups)
                       .HasForeignKey(d => d.WingMawId)
                       .OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
