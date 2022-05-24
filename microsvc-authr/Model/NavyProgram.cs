using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_authr.Model
{
    public class NddsProgram
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? LongName { get; set; }
        public virtual ICollection<OrgProgram> ProgramOrgs { get; set; }
    }

    public class NddsProgramConfig : IEntityTypeConfiguration<NddsProgram>
    {
        public void Configure(EntityTypeBuilder<NddsProgram> builder)
        {
            builder.ToTable("Programs");
            builder.HasIndex(e => e.Id);
            builder.HasIndex(e => e.Name);;
        }
    }
}
