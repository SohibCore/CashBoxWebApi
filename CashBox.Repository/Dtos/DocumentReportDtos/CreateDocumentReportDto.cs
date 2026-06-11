using CashBox.Repository.Entity.Reports;
using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.DocumentReportDto
{
    public class CreateDocumentReportDto
    {
        [Range(1, int.MaxValue)]
        public int SupplierId { get; set; }

        [Required]
        [Range(0.0000000000000000001, int.MaxValue)]
        public decimal Debit { get; set; }

        [Required]
        [Range(0.0000000000000000001, int.MaxValue)]
        public decimal Credit { get; set; }

        [Required]
        public decimal OpeningDebit { get; set; }

        [Required]
        public decimal OpeningCredit { get; set; }

        [Required]
        public decimal ClosingDebit { get; set; }

        [Required]
        public decimal ClosingCredit { get; set; }

        public List<IncomeReportTable> IncomeReports { get; set; } = new List<IncomeReportTable>();
        public List<OutcomeReportTable> OutcomeReports { get; set; } = new List<OutcomeReportTable>();
    }
}
