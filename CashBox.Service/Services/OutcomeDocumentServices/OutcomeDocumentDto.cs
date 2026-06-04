using CashBox.Service.Services.OutcomeDocumentServices;

namespace CashBox.Repository.Dtos.OutcomeDocumentDtos
{
    public class OutcomeDocumentDto
    {
        public long Id { get; set; }
        public DateTime DocOn { get; set; }
        public int SupplierId { get; set; }
        public int ProductId { get; set; }
        public decimal Price { get; set; }
        public decimal Quantity { get; set; }
        public decimal DocSum { get; set; }

        public ICollection<OutcomeDocumentTableDto> Tables = new List<OutcomeDocumentTableDto>();
    }
}
