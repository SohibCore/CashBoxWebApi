namespace CashBox.Integrations.UzasboServices.Dtos
{
    public class WeatherResponseDto
    {
        public string Name { get; set; } = null!;

        public MainDto Main { get; set; } = null!;
    }
    public class MainDto
    {
        public decimal Temp { get; set; }
    }
}
