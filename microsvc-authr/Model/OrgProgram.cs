using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace microsvc_authr.Model
{
    public class OrgProgram
    {
        public int OrganizationId { get; set; }
        public virtual Organization Org { get; set; }
        public int ProgramId { get; set; }
        public virtual NddsProgram Program { get; set; }
    }

    public class OrgProgamConfig : IEntityTypeConfiguration<OrgProgram>
    {
        public void Configure(EntityTypeBuilder<OrgProgram> builder)
        {
            builder.ToTable("OrgProgams");
            builder.HasKey(ur => new { ur.OrganizationId, ur.ProgramId });

            builder.HasOne(a => a.Org)
                   .WithMany(b => b.OrgPrograms)
                   .HasForeignKey(c => c.OrganizationId);

            builder.HasOne(a => a.Program)
                   .WithMany(b => b.ProgramOrgs)
                   .HasForeignKey(c => c.ProgramId);  

        }
    }
}
