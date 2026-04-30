using CashBox.Repository.Dtos.CorrencyDtos;

namespace CashBox.Service.Services.CorrencyServices
{
    public interface ICorrencyService
    {
        Task<CorrencyDto> GetAsync(int id);
        Task CreateAsync(CreateCorrencyDto createCorrencyDto);
        Task UpdateAsync(int id, UpdateCorrencyDto updateCorrencyDto);
        Task DeleteAsync(int id);
    }
}
