namespace CashBox.Repository.Dtos.ProductDtos
{
    public class ProductFilterDto
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
        public string? Code { get; set; }
        public int? OrganizationId { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
