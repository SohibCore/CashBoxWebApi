using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.OutcomeDocumentDtos
{
    public class CreateOutcomeDocumentDlDto
    {
        [Required]
        public DateTime DocOn { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }

        public List<CreateOutcomeDocumentTableDlDto> Tables { get; set; } = new();
    }
}
