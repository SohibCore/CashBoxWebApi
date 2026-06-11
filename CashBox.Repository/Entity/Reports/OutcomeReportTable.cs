using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Entity.Reports
{
    [Table("OUTCOME_REPORT")]
    public class OutcomeReportTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("SUPPLIER_ID")]
        public int SupplierId { get; set; }

        [Column("PRODUCT_ID")]
        public int ProductId { get; set; }

        [Column("PRICE")]
        public decimal Price { get; set; }

        [Column("QUANTITY")]
        public decimal Quantity { get; set; }

        [Column("DEBIT")]
        public decimal Debit { get; set; }

        [Column("CREDIT")]
        public decimal Credit { get; set; }

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
    }
}
