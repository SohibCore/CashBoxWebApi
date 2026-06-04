namespace CashBox.Service.Services.OutcomeDocumentServices
{
    public class OutcomeDocumentTableDto
    {
        public long Id { get; set; }
        public long OwnerId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal TotalSum { get; set; }
    }
}
