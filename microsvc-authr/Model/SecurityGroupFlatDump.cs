using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class SecurityGroupFlatDump
    {
        public int Id { get; set; }
        public int Buno { get; set; }
        public string Tms { get; set; }
        public string TypeModel { get; set; }
        public string Org { get; set; }
        public string OrgCode { get; set; }
        public string Location { get; set; }
        public string WingMaw { get; set; }
        public string WingMawCode { get; set; }
        public DateTime QtrEndDate { get; set; }
        public virtual ICollection<UserSecurityGroup> SecurityGroupUsers { get; set; }
    }

    public class SecurityGroupFlatDumpConfig : IEntityTypeConfiguration<SecurityGroupFlatDump>
    {
        public void Configure(EntityTypeBuilder<SecurityGroupFlatDump> builder)
        {
            builder.ToTable("SecurityGroupFlatDumps");
            builder.HasIndex(e => e.Buno);
        }
    }
}
