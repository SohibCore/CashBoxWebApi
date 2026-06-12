using System.Net.Http.Headers;
using CashBox.AdminPanel.Helpers;

namespace CashBox.AdminPanel.Services
{
    public static class ApiClient
    {
        private static readonly string BaseUrl = "https://localhost:5107/";

        private static readonly HttpClientHandler _handler = new HttpClientHandler
        {
            ServerCertificateCustomValidationCallback = (message, cert, chain, errors) => true
        };

        private static readonly HttpClient _client = new HttpClient(_handler)
        {
            BaseAddress = new Uri(BaseUrl)
        };

        public static HttpClient Instance
        {
            get
            {
                if (!string.IsNullOrEmpty(SessionManager.Token))
                {
                    _client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", SessionManager.Token);
                }
                else
                {
                    _client.DefaultRequestHeaders.Authorization = null;
                }
                return _client;
            }
        }
    }
}