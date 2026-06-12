namespace CashBox.Service.Services.DocumentReportServices
{
    public class DocumentReportFilterDto
    {
        public int? Id { get; set; }
        public string? Inn { get; set; } = null!;
        public decimal? Price { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public decimal? OpeningDebit { get; set; }
        public decimal? OpeningCredit { get; set; }
        public int? SupplierId { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
    }
}
