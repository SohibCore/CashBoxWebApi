using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.ContractorAccount
{
    public class CreateContractorAccountDto
    {
        [Required]
        [StringLength(27)]
        public string Code { get; set; } = null!;

        [Range(1, int.MaxValue)]
        public int ContractorId { get; set; }

        [Required]
        [StringLength(500)]
        public string FullName { get; set; } = null!;
    }
}
