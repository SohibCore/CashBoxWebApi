using CashBox.Repository.Dtos.OrganizationDtos;

namespace CashBox.Service.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        Task<OrganizationDto> GetAsync(int id);
        Task<List<OrganizationDto>> GetListAsync(OrganizationFilterDto organizationFilterDto);
        Task CreateAsync(CreateOrganizationDto createOrganizationDto);
        Task UpdateAsync(int id, UpdateOrganizationDto updateOrganizationDto);
        Task DeleteAsync(int id);
    }
}
