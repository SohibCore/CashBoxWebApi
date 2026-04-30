using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.ContractorAccount
{
    public class ContractorAccountDto
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; } = null!;
        public int ContractorId { get; set; }

        [Required]
        public string FullName { get; set; } = null!;
    }
}
