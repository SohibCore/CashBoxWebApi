using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.CorrencyDtos
{
    public class CreateCorrencyDto
    {
        [Required]
        [StringLength(9)]
        public string Code { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string ShortName { get; set; } = null!;
    }
}
