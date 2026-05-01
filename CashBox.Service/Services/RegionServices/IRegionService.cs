using CashBox.Repository.Dtos.RegionDtos;

namespace CashBox.Service.Services.RegionServices
{
    public interface IRegionService
    {
        Task<RegionDto> GetAsync(int id);
        Task<List<RegionDto>> GetListAsync(string searchRegion, RegionFilterDto regionFilterDto);
        Task CreateAsync(CreateRegionDto createRegionDto);
        Task UpdateAsync(int id, UpdateRegionDto updateRegionDto);
        Task DeleteAsync(int id);
    }
}