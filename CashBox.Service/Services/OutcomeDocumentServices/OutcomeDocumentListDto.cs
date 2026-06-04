namespace CashBox.Service.Services.OutcomeDocumentServices
{
    public class OutcomeDocumentListDto
    {
        public long Id { get; set; }
        public DateTime DocOn { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public string StatusName { get; set; } = null!;
        public int StatusId { get; set; }
        public decimal DocSum { get; set; }
    }
}
