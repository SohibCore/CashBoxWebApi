namespace CashBox.Service.Services.IncomeDocumentServices
{
    public class IncomeDocumentDto
    {
        public long Id { get; set; }
        public DateTime DocOn { get; set; }
        public string Inn { get; set; } = null!;
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public decimal DocSum { get; set; }

        public List<IncomeDocumentTableDto> Tables { get; set; } = new List<IncomeDocumentTableDto>();
    }
}
