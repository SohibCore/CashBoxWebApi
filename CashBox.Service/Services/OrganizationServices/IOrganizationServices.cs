using CashBox.Repository.Dtos.OrganizationDtos;

namespace CashBox.Service.Services.OrganizationServices
{
    public interface IOrganizationService
    {
        Task<OrganizationDto> GetAsync(int id);
        Task CreateAsync(CreateOrganizationDto createOrganizationDto);
        Task UpdateAsync(int id, UpdateOrganizationDto updateOrganizationDto);
        Task DeleteAsync(int id);
    }
}
