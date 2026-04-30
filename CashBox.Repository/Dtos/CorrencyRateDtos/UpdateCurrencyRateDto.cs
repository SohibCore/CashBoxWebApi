namespace CashBox.Repository.Dtos.CorrencyRateDtos
{
    public class UpdateCurrencyRateDto
    {
        public int? CurrencyId { get; set; }
        public decimal? Rate { get; set; }
        public DateTime? Date { get; set; }
    }
}
