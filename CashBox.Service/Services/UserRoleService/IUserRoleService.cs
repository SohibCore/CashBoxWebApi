using CashBox.Repository.Dtos.UserRoleDtos;

namespace CashBox.Service.Services.UserRoleService
{
    public interface IUserRoleService
    {
        Task AssignAsync(int userId, int roleId);
        Task RemoveAsync(int userId, int roleId);
        Task<List<UserRoleDto>> GetListAsync(UserRoleFilter userRoleFilter);
    }
}
