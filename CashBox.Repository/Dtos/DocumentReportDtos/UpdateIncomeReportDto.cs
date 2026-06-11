namespace CashBox.Repository.Dtos.DocumentReportDtos
{
    public class UpdateIncomeReportDto
    {
        public long Id { get; set; }
        public int SupplierId { get; set; }
        public string SupplierName { get; set; } = null!;
        public int ProductId { get; set; }
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
    }
}
