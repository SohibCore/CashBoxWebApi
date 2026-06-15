using CashBox.Integrations.UzasboServices.Dtos;

namespace CashBox.Service.Integrations.WeatherServices
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetWeatherAsync(string name);
    }
}
