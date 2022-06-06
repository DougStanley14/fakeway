using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace microsvc_userprofile.Model
{
    public class NddsProgram
    {
        public NddsProgram()
        {
            Orgs = new List<Organization>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public virtual ICollection<Organization> Orgs { get; set; }
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
