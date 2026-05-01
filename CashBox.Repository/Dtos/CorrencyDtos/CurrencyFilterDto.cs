namespace CashBox.Repository.Dtos.CorrencyDtos
{
    public class CurrencyFilterDto
    {
        public int? Id { get; set; }
        public string? Code { get; set; } 
        public string? FullName { get; set; } 
        public string? ShortName { get; set; } 
    }
}
