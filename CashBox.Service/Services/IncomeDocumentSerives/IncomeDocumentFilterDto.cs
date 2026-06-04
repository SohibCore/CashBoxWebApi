
namespace CashBox.Service.Services.IncomeDocumentServices
{
    public class IncomeDocumentFilterDto
    {
        public int? Id { get; set; }
        public int? SupplierId { get; set; }
        public int? ProductId { get; set; }
        public decimal? DocSum { get; set; }

        public int? StatusId { get; set; }
    }
}
