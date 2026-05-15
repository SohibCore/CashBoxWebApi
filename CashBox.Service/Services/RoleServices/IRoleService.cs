using CashBox.Repository.Dtos.RoleDtos;
using CashBox.Repository.Entity;

namespace CashBox.Service.Services.NewFolder
{
    public interface IRoleService
    {
        Task CreateAsync(CreateRoleDto createRoleDto);
        Task<RoleDto> GetAsync(int id);
        Task<List<RoleDto>> GetListAsync(RoleFilterDto roleFilterDto);
        Task UpdateAsync(int id, UpdateRoleDto updateRoleDto);
        Task DeleteAsync(int id);
    }
}
