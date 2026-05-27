using CashBox.Repository.Enums;

namespace CashBox.Repository.Dtos.IncomeDocumentDtos
{
    public class UpdateIncomeDocument
    {
        public DateTime? Date { get; set; }
        public int? SupplierId { get; set; }
        public int? ProductId { get; set; }
        //public int? OrganizationId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalSum { get; set; }
        public PaymentStatus? Status { get; set; }
    }
}
