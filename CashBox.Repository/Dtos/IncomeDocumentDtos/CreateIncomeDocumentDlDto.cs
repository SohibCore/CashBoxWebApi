using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.IncomeDocumentDtos
{
    public class CreateIncomeDocumentDlDto
    {
        [Required]
        public DateTime DocOn { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }

        [Required]
        [Range(0.0000000000000000001, int.MaxValue)]
        public decimal DocSum { get; set; }
        public List<IncomeDocumentTableDlDto> Tables { get; set; } = new List<IncomeDocumentTableDlDto>();
    }
}
