
namespace CashBox.Service.Services.IncomeDocumentServices
{
    public class IncomeDocumentListDto
    {
        public long Id { get; set; }
        public DateTime DocOn { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public int StatusId { get; set; } 
        public decimal DocSum { get; set; }
    }
}
