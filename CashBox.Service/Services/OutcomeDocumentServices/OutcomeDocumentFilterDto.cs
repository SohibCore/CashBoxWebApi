namespace CashBox.Service.Services.OutcomeDocumentService
{
    public class OutcomeDocumentFilterDto
    {
        public int? Id { get; set; }
        public int? SupplierId { get; set; }
        public DateTime? DocOn { get; set; }
        public int? ProductId { get; set; }
        public string? ProductName { get; set; }
        public decimal? DocSum { get; set; }
        public int? StatusId { get; set; }
    }
}
