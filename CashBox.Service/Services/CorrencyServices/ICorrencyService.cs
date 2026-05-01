using CashBox.Repository.Dtos.CorrencyDtos;

namespace CashBox.Service.Services.CorrencyServices
{
    public interface ICurrencyService
    {
        Task<CorrencyDto> GetAsync(int id);
        Task<List<CorrencyDto>> GetListAsync(CurrencyFilterDto currencyFilterDto);
        Task CreateAsync(CreateCorrencyDto createCorrencyDto);
        Task UpdateAsync(int id, UpdateCorrencyDto updateCorrencyDto);
        Task DeleteAsync(int id);
    }
}
