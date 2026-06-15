using CashBox.Integrations.UzasboServices.Dtos;
using CashBox.Service.Configurations;
using System.Text.Json;

namespace CashBox.Service.Integrations.UzasboServices
{
    public class UzasboService : IUzasboService
    {
        private readonly HttpClient _httpClient;
        private readonly UzasboSetting _settings;

        public UzasboService(HttpClient httpClient, UzasboSetting settings)
        {
            _httpClient = httpClient;
            _settings = settings;
        }

        public async Task<UserByInnResponseDto> GetFacturaAsync(string tin)
        {
            _httpClient.DefaultRequestHeaders.Clear();

            _httpClient.DefaultRequestHeaders.Add(
                "Authorization",
                _settings.Authorization);

            var response = await _httpClient.GetAsync(
                $"{_settings.BaseUrl}/soliq/services/np1/bytin/factura-all?tin={tin}");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonSerializer.Deserialize<UserByInnResponseDto>(content, new JsonSerializerOptions
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
