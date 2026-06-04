using CashBox.Repository.Enum;
using RepositoryLayer.Entity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Entity
{
    [Table("INCOME_DOCUMENT")]
    public class IncomeDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("DOC_ON")]
        public DateTime DocOn { get; set; }

        [Column("SUPPLIER_ID")]
        public int SupplierId { get; set; }        

        [Column("STATUS_ID")]
        public int StatusId { get; set; } 

        [Column("DOC_SUM")]
        public decimal DocSum { get; set; }

        [Column("ORGANIZATION_ID")]
        public int? OrganizationId { get; set; }

        [Column("CREATE_USER_ID")]
        public int? CreateUserId { get; set; }
        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }
        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }
        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

        [ForeignKey(nameof(StatusId))]
        public Status Status { get; set; } = null!;

        [ForeignKey(nameof(OrganizationId))]
        public Organization Organization { get; set; } = null!;
        public ICollection<IncomeDocumentTable> Tables { get; set; } = new List<IncomeDocumentTable>();
    }
}
