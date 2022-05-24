using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class NddsOrgSysProg
    {
        public int NddsOrgId { get; set; }
        public virtual NddsOrg NddsOrg { get; set; }
        public int SysProgId { get; set; }
        public virtual SysProg SysProg { get; set; }
    }

    public class NddsOrgSysProgConfig : IEntityTypeConfiguration<NddsOrgSysProg>
    {
        public void Configure(EntityTypeBuilder<NddsOrgSysProg> builder)
        {
            builder.ToTable("NddsOrgSysProgs");
            builder.HasKey(ur => new { ur.NddsOrgId, ur.SysProgId });

            builder.HasOne(a => a.NddsOrg)
                   .WithMany(b => b.OrgPrograms)
                   .HasForeignKey(c => c.NddsOrgId);

            builder.HasOne(a => a.SysProg)
                   .WithMany(b => b.SysProgOrgs)
                   .HasForeignKey(c => c.SysProgId);  

        }
    }
}
