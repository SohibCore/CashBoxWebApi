namespace CashBox.Repository.Dtos.SupplierDtos
{
    public class SupplierFilterDto
    {
        public int? Id { get; set; }
        public string? Inn { get; set; }
        public string? Code { get; set; }
        public int? OrganizationId { get; set; }
    }
}
