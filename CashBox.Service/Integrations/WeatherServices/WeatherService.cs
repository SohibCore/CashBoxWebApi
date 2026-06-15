using CashBox.Integrations.UzasboServices.Dtos;
using Microsoft.Extensions.Configuration;
using System.Text.Json;

namespace CashBox.Service.Integrations.WeatherServices
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _config = configuration;
        }

        public async Task<WeatherResponseDto> GetWeatherAsync(string name)
        {
            var apiKey = _config["OpenWeather:ApiKey"];

            var response = await _httpClient.GetAsync(
                $"https://api.openweathermap.org/data/2.5/weather?q={name}&appid={apiKey}&units=metric");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<WeatherResponseDto>(    // JSON ni C# obyektiga aylantirish
                content,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

            if (result != null)
                return result;
            else
                throw new Exception("Ma'lumot topilmadi");
        }
    }
}
