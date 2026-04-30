using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.ContractorDtos
{
    public class CreateContractorDto
    {
        [Required]
        [StringLength(9)]
        public string Inn { get; set; } = null!;

        [Required]
        [StringLength(14)]
        public string Pinfl { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string ShortName { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string FullName { get; set; } = null!;
        public int RegionId { get; set; }
        public int DistrictId { get; set; }
    }
}
