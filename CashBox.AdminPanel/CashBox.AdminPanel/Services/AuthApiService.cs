using System.Net.Http.Json;
using static CashBox.AdminPanel.Models.AuthModels;

namespace CashBox.AdminPanel.Services
{
    public class AuthApiService
    {
        public async Task<(bool Success, string Message)> LoginAsync(string userName, string password)
        {
            try
            {
                var request = new LoginRequest { UserName = userName, Password = password };
                var response = await ApiClient.Instance.PostAsJsonAsync("api/auth/login", request);

                if (!response.IsSuccessStatusCode)
                {
                    var error = await response.Content.ReadAsStringAsync();
                    return (false, $"Login xato: {response.StatusCode} - {error}");
                }

                var result = await response.Content.ReadFromJsonAsync<LoginResponse>();
                if (result == null || string.IsNullOrEmpty(result.Token))
                    return (false, "Token olinmadi.");

                Helpers.SessionManager.Token = result.Token;
                Helpers.SessionManager.UserName = userName;
                return (true, "OK");
            }
            catch (HttpRequestException ex)
            {
                return (false, $"Serverga ulanishda xatolik: {ex.Message}");
            }
        }
    }
}