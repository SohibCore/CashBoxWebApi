using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.DistrictDtos
{
    public class CreateDistrictDto
    {
        [Required]
        [StringLength(500)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(9)]
        public string Code { get; set; } = null!;

        [Required]
        public string Region { get; set; } = null!;
    }
}
