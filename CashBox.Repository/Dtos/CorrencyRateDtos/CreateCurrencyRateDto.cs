using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.CorrencyRateDtos
{
    public class CreateCurrencyRateDto
    {
        [Range(1, int.MaxValue)]
        public int CurrencyId { get; set; }

        [Range(0.0000001, double.MaxValue)]
        public decimal Rate { get; set; }
        public DateTime Date { get; set; }
    }
}
