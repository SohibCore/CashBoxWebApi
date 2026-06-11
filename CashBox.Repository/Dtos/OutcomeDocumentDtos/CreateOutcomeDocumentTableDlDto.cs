using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.OutcomeDocumentDtos
{
    public class CreateOutcomeDocumentTableDlDto
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int ProductId { get; set; }

        [Required]
        [Range(0.0000000000000000001, double.MaxValue)]
        public decimal Price { get; set; }

        [Required]
        [Range(0.0000000000000000001, double.MaxValue)]
        public decimal Quantity { get; set; }
    }
}
