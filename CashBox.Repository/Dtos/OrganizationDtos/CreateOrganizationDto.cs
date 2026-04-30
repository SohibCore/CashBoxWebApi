using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.OrganizationDtos
{
    public class CreateOrganizationDto
    {
        [Required]
        [StringLength(9)]
        public string Inn { get; set; } = null!;

        [Required]
        [StringLength(500)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string ShortName { get; set; } = null!;

        [Range(1,int.MaxValue)]
        public int RegionId { get; set; }

        [Required]
        [StringLength(300)]
        public string District { get; set; } = null!;
    }
}
