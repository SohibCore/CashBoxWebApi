using CashBox.Repository.Dtos.OrganizationDtos;

namespace CashBox.Repository.Dtos.SupplierDtos
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Inn { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Name { get; set; } = null!;
        public string Address { get; set; } = null!;
        public string? Mfo { get; set; } 
        public string? Account { get; set; }
        public string? Director { get; set; }
    }
}
