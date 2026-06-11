using CashBox.Service.Services.OutcomeDocumentServices;

namespace CashBox.Repository.Dtos.OutcomeDocumentDtos
{
    public class OutcomeDocumentDto
    {
        public long Id { get; set; }
        public DateTime DocOn { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal DocSum { get; set; }
        public string SupplierName { get; set; } = null!;
        public string StatusName { get; set; } = null!;

        public ICollection<OutcomeDocumentTableDto> Tables { get; set; } = new List<OutcomeDocumentTableDto>();
    }
}
