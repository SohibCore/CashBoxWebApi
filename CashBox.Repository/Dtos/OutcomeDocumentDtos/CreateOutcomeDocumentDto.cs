using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.OutcomeDocumentDtos
{
    public class CreateOutcomeDocumentDto
    {
        [Required]
        public DateTime Date { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(0.0000000000000000001, int.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0.0000000000000000001, int.MaxValue)]
        public decimal Quantity { get; set; }

        [Required]
        [Range(0.0000000000000000001, int.MaxValue)]
        public decimal TotalSum { get; set; }
    }
}
