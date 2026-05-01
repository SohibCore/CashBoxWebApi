using CashBox.Repository.Dtos.CorrencyRateDtos;

namespace CashBox.Service.Services.CorrencyRateServices
{
    public interface ICurrencyRateService
    {
        Task<CurrencyRateDto> GetAsync(int id);
        Task<List<CurrencyRateDto>> GetListAsync(CurrencyRateFilterDto currencyRateFilterDto);
        Task CreateAsync(CreateCurrencyRateDto createCurrencyRateDto);
        Task UpdateAsync(int id, UpdateCurrencyRateDto updateCurrencyRateDto);
        Task DeleteAsync(int id);
    }
}
