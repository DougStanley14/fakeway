using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class SecurityGroup
    {
        public int Id { get; set; }
        public int Buno { get; set; }
        public string Tms { get; set; }
        public string TypeModel { get; set; }
        public string Org { get; set; }
        public string OrgCode { get; set; }
        //public string Location { get; set; }
        public string WingMaw { get; set; }
        public string WingMawCode { get; set; }
        public DateTime QtrEndDate { get; set; }
        public virtual ICollection<NddsUser> SecurityGroupUsers { get; set; }
    }

    public class SecurityGroupConfig : IEntityTypeConfiguration<SecurityGroup>
    {
        public void Configure(EntityTypeBuilder<SecurityGroup> builder)
        {
            builder.ToTable("SecurityGroups");
            builder.HasIndex(e => e.Buno);
        }
    }
}
