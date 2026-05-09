using CashBox.Repository.Dtos.UserRoleDtos;
using CashBox.Repository.Entity;

namespace CashBox.Service.Services.UserRoleService
{
    public interface IUserRoleService
    {
        Task<List<UserRole>> GetListAsync(UserRoleFilter userRoleFilter);
        Task<UserRole> GetAsync(int id);
        Task CreateAsync(UserRole userRole);
        Task DeleteAsync(int id);
    }
}
