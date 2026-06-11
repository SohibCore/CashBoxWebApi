using CashBox.Repository.Enum;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashBox.Repository.Entity.Reports
{
    [Table("DOCUMENT_REPORT")]
    public class DocumentReport
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]

        [Column("ID")]
        public long Id { get; set; }

        [Column("SUPPLIER_ID")]
        public int SupplierId { get; set; }

        [Column("OPENING_DEBIT")]
        public decimal OpeningDebit { get; set; }

        [Column("OPENING_CREDIT")]
        public decimal OpeningCredit { get; set; }

        [Column("ORGANIZATION_ID")]
        public int OrganizationId { get; set; }

        [Column("DEBIT")]
        public decimal Debit { get; set; }

        [Column("CREDIT")]
        public decimal Credit { get; set; }

        [Column("CLOSING_DEBIT")]
        public decimal ClosingDebit { get; set; }

        [Column("CLOSING_CREDIT")]
        public decimal ClosingCredit { get; set; }

        [Column("STATUS_ID")]
        public int StatusId { get; set; }

        [Column("CREATE_USER_ID")]
        public int? CreateUserId { get; set; }

        [Column("CREATED_AT")]
        public DateTime? CreatedAt { get; set; }

        [Column("MODIFIED_USER_ID")]
        public int? ModifiedUserId { get; set; }

        [Column("MODIFIED_AT")]
        public DateTime? ModifiedAt { get; set; }

        [ForeignKey(nameof(SupplierId))]
        public Supplier Supplier { get; set; } = null!;

        [ForeignKey(nameof(StatusId))]
        public Status Status { get; set; } = null!;

        public List<IncomeReportTable> IncomeReport { get; set; } = new List<IncomeReportTable>();
        public List<OutcomeReportTable> OutcomeReport { get; set; } = new List<OutcomeReportTable>();
    }
}
