namespace CashBox.Repository.Dtos.ProductDtos
{
    public class ProductListDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string Code { get; set; } = null!;
        public int OrganizationId { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}
