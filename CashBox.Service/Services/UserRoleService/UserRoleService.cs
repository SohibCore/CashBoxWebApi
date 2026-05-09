using CashBox.Repository.Dtos.UserRoleDtos;
using CashBox.Repository.Entity;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.UserRoleService
{
    public class UserRoleService : IUserRoleService
    {
        private readonly AppDbContext _context;
        public UserRoleService(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(UserRole userRole)
        {
            var role = _context.UserRoles.AnyAsync(x => x.UserId == userRole.UserId && x.RoleId == userRole.RoleId);

            if (role != null)
                throw new Exception("Bu role allaqachon biriktirilgan");

            var roles = new UserRole
            {
                UserId = userRole.UserId,
                RoleId = userRole.RoleId,
                StateId = userRole.StateId,
            };
            await _context.UserRoles.AddAsync(roles);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);

            if (userRole == null)
                throw new Exception("Bu id li userRole mavjud emas");

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
        public async Task<UserRole> GetAsync(int id)
        {
            var userRole = await _context.UserRoles.FindAsync(id);

            if (userRole == null)
                throw new Exception("Bu id li userRole mavjud emas");

            return new UserRole
            {
                Id = userRole.Id,
                UserId = userRole.UserId,
                RoleId = userRole.RoleId,
                StateId = userRole.StateId,
            };
        }
        public async Task<List<UserRole>> GetListAsync(UserRoleFilter userRoleFilter)
        {
            var roles = _context.UserRoles.AsQueryable();

            if (userRoleFilter.Id != 0 || userRoleFilter.Id == null)
                roles = roles.Where(x => x.Id == userRoleFilter.Id);

            if (userRoleFilter.UserId != 0 || userRoleFilter.UserId == null)
                roles = roles.Where(x => x.UserId == userRoleFilter.UserId);

            if (userRoleFilter.RoleId != 0 || userRoleFilter.RoleId == null)
                roles = roles.Where(x => x.RoleId == userRoleFilter.RoleId);

            return await roles.Select(u => new UserRole
            {
                Id = u.Id,
                UserId = u.UserId,
                RoleId = u.RoleId,
                StateId = u.StateId,
            }).ToListAsync();
        }
    }
}
