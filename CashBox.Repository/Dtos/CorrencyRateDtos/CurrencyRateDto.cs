namespace CashBox.Repository.Dtos.CorrencyRateDtos
{
    public class CurrencyRateDto
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
