using CashBox.Repository.Dtos.RegionDtos;

namespace CashBox.Service.Services.RegionServices
{
    public interface IRegionService
    {
        Task<RegionDto> GetAsync(int id);
        Task CreateAsync(CreateRegionDto createRegionDto);
        Task UpdateAsync(int id, UpdateRegionDto updateRegionDto);
        Task DeleteAsync(int id);
    }
}