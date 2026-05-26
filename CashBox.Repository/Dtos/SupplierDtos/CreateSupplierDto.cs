using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.SupplierDtos
{
    public class CreateSupplierDto
    {
        [Required]
        public string Code { get; set; } = null!;

        [Required]
        [StringLength(10)]
        public string Inn { get; set; } = null!;
    }
}
