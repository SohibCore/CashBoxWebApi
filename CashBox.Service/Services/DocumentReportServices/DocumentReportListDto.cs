namespace CashBox.Service.Services.DocumentReportServices
{
    public class DocumentReportListDto
    {
        public long Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string Inn { get; set; } = null!;
        public decimal OpeningDebit { get; set; }
        public decimal OpeningCredit { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal ClosingDebit { get; set; }
        public decimal ClosingCredit { get; set; }
        public decimal Price { get; set; }
    }
}
