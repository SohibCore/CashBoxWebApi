using CashBox.Integrations.UzasboServices.Dtos;
namespace CashBox.Service.Integrations.UzasboServices
{
    public interface IUzasboService
    {
        Task<UserByInnResponseDto> GetFacturaAsync(string tin);
    }
}
