namespace CashBox.Repository.Dtos.DocumentReportDtos
{
    public class UpdateDocumentReportDlDto
    {
        public long Id { get; set; }
        public int SupplierId { get; set; }
        public decimal OpeningDebit { get; set; }
        public decimal OpeningCredit { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public decimal ClosingDebit { get; set; }
        public decimal ClosedCredit { get; set; }
        public List<UpdateIncomeReportDto> IncomeUpdate { get; set; } = new List<UpdateIncomeReportDto>();
        public List<UpdateOutcomeReportDto> OutcomeUpdate { get; set; } = new List<UpdateOutcomeReportDto>();
    }
}
