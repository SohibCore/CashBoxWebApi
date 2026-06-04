using CashBox.Repository.Entity;
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
        public decimal DocSum { get; set; }
        public ICollection<IncomeDocumentTableDlDto> Tables { get; set; } = new List<IncomeDocumentTableDlDto>();

    }
}
