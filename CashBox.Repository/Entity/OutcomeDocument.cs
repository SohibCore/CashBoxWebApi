using CashBox.Repository.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Entity
{
    [Table("OUTCOME_DOCUMENT")]
    public class OutcomeDocument
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("DOC_ON")]
        public DateTime DocOn { get; set; }

        [Column("SUPPLIER_ID")]
        public int SupplierId { get; set; }

        [Column("PRODUCT_ID")]
        public int ProductId { get; set; }

        [Column("QUANTITY")]
        public decimal Quantity { get; set; }

        [Column("ORGANIZATION_ID")]
        public int OrganizationId { get; set; }

        [Column("PRICE")]
        public decimal Price { get; set; }

        [Column("DOC_SUM")]
        public decimal DocSum { get; set; }

        [Column("STATUS_ID")]
        public int StatusId { get; set; }
        [Column("CREATE_USER_ID")]
        public int? CreateUserId { get; set; }
        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }
        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }
        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(StatusId))]
        public Status Status { get; set; } = null!;

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; } = null!;

        public ICollection<OutcomeDocumentTable> Tables { get; set; } = new List<OutcomeDocumentTable>();
    }
}
