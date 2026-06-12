namespace CashBox.AdminPanel.Models
{
    public class AuthModels
    {
        public class LoginRequest
        {
            public string UserName { get; set; } = null!;
            public string Password { get; set; } = null!;
        }

        public class LoginResponse
        {
            public string Token { get; set; } = null!;
        }
    }
}
