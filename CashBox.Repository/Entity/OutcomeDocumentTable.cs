using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Entity
{
    [Table("OUTCOME_DOCUMENT_TABLE")]
    public class OutcomeDocumentTable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("OWNER_ID")]
        public long OwnerId { get; set; }

        [Column("PRODUCT_ID")]
        public int ProductId { get; set; }

        [Column("PRICE")]
        public decimal Price { get; set; }

        [Column("QUANTITY")]
        public decimal Quantity { get; set; }

        [Column("TOTAL_SUM")]
        public decimal TotalSum { get; set; }

        [ForeignKey(nameof(OwnerId))]
        public OutcomeDocument OutcomeDocument { get; set; } = null!;

        [ForeignKey(nameof(ProductId))]
        public Product Product { get; set; } = null!;
    }
}
