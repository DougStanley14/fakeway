using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ndds.model.Transactions
{
    public class Operation
    {
        public Operation()
        {
        }

        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public long UserEdipi { get; set; }
        public DateTime TimeStamp { get; set; }
        public OperationType OperationType { get; set; }

        [MaxLength(100)]
        public string? OperationUrl { get; set; }
        [MaxLength(50)]
        public string? EntityName { get; set; }  // I.e Name of Table/Entity
        public ChangeDataType ChangeDataType { get; set; }
        public ChangeDataFormat ChangeDataFormat { get; set; }
        public string? ChangeData { get; set; }
        public string? BaseObject { get; set; }
    }



    public class OperationConfig : IEntityTypeConfiguration<Operation>
    {
        public void Configure(EntityTypeBuilder<Operation> builder)
        {
            builder.ToTable("Operations");
            builder.HasIndex(e => e.Id);

            builder.Property(p => p.OperationType)
                    .HasConversion<string>();

            builder.Property(p => p.ChangeDataType)
                    .HasConversion<string>();

            builder.Property(p => p.ChangeDataFormat)
                    .HasConversion<string>();

        }
    }



    public enum OperationType
    {
        Create,
        Read,
        Update,
        Delete,
    }
    public enum ChangeDataType
    {
        JSON,
        BSON
    }

    public enum ChangeDataFormat
    {
        SerializedObject,
        JsonDiffPatch,
        JsonDiff
    }




}
