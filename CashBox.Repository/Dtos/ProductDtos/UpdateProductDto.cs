namespace CashBox.Repository.Dtos.ProductDtos
{
    public class UpdateProductDto
    {
        public string? Code { get; set; }
        public string? Name { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
