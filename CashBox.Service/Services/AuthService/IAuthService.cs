namespace CashBox.Service.Services.AuthService
{
    public interface IAuthService
    {
        Task RegisterAsync(RegisterDto dto);
        Task<string> LoginAsync(LoginDto dto);
        Task RefreshAsync(LoginDto dto);
    }
}
