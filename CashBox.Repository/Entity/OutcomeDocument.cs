using CashBox.Repository.Enums;
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
        public int Id { get; set; }

        [Column("DATE")]
        public DateTime Date { get; set; }

        [Column("SUPPLIER_ID")]
        public int SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        public Supplier? Supplier { get; set; }

        [Column("PRODUCT_ID")]
        public int ProductId { get; set; }
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [Column("QUANTITY")]
        public decimal Quantity { get; set; }

        [Column("PRICE")]
        public decimal Price { get; set; }

        [Column("TOTAL_SUM")]
        public decimal TotalSum { get; set; }

        [Column("STATUS")]

        public PaymentStatus Status { get; set; } = PaymentStatus.Unpaid; // enum bo'lgani uchun
    }
}
