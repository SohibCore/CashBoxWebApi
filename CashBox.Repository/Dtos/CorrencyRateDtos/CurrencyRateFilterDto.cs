namespace CashBox.Repository.Dtos.CorrencyRateDtos
{
    public class CurrencyRateFilterDto
    {
        public int? Id { get; set; }
        public int? CurrencyId { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? Date { get; set; }
    }
}
