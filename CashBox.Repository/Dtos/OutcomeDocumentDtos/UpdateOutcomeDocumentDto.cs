namespace CashBox.Repository.Dtos.OutcomeDocumentDtos
{
    public class UpdateOutcomeDocumentDto
    {
        public DateTime? Date { get; set; }
        public int? SupplierId { get; set; }
        public int? ProductId { get; set; }
        public decimal? Price { get; set; }
        public decimal? Quantity { get; set; }
        public decimal? TotalSum { get; set; }
    }
}
