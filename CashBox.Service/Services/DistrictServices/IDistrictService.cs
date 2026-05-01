using CashBox.Repository.Dtos.DistrictDtos;

namespace CashBox.Service.Services.DistrictServices
{
    public interface IDistrictService
    {
        Task<DistrictDto> GetAsync(int id);
        Task<List<DistrictDto>> GetListAsync(DistrictFilterDto districtFilterDto);
        Task CreateAsync(CreateDistrictDto createDistrictDto);
        Task UpdateAsync(int id, UpdateDistrictDto updateDistrictDto);
        Task DeleteAsync(int id);
    }
}
