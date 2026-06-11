namespace CashBox.Service.Services.DocumentReportServices
{
    public class IncomeReport
    {
        //public long Id { get; set; }
        //public long OwnerId { get; set; }
        //public int ProductId { get; set; }
        //public string ProductName { get; set; } = null!;
        //public int SupplierId { get; set; }
        //public string SupplierName { get; set; } = null!;
        //public string Inn { get; set; } = null!;
        //public decimal Price { get; set; }
        //public decimal Quantity { get; set; }
        //public decimal Debit { get; set; }
        //public decimal Credit { get; set; }

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
