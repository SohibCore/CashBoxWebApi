namespace CashBox.Service.Services.DocumentReportServices
{
    public class DocumentReportDto
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

        public List<IncomeReport> InReports = new List<IncomeReport>();
        public List<OutcomeReport> OutReports = new List<OutcomeReport>();
    }
}
