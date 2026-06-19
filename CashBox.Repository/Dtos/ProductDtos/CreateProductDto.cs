using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.ProductDtos
{
    public class CreateProductDto
    {
        [Required]
        public string Name { get; set; } = null!;

        [Required]
        public string Code { get; set; } = null!;

        [Required]
        public int OrganizationId { get; set; }

        [Required]
        public DateTime DeliveredAt { get; set; }
        public int CreatedUserId { get; set; }
        public DateTime CreateAt { get; set; }

    }
}
