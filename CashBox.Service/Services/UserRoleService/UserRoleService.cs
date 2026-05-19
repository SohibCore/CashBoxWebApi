using CashBox.Core.Roles;
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

        //public async Task AssignAsync(int userId, int roleId)
        //{
        //    var role = await _context.UserRoles
        //   .AnyAsync(x => x.UserId == userId && x.RoleId == roleId);

        //    if (role)
        //        throw new InvalidOperationException("Role allaqachon yuklangan");

        //    var userRole = new UserRole
        //    {
        //        UserId = userId,
        //        RoleId = roleId
        //    };

        //    await _context.UserRoles.AddAsync(userRole);
        //    await _context.SaveChangesAsync();
        //}
        public async Task AssignAsync(int userId, List<int> roleIds)
        {
            foreach (var roleId in roleIds)
            {
                var exists = await _context.UserRoles
                    .AnyAsync(x => x.UserId == userId && x.RoleId == roleId);

                if (exists) continue;  // bor bo'lsa o'tkazib yuboramiz

                var userRole = new UserRole
                {
                    UserId = userId,
                    RoleId = roleId
                };
                await _context.UserRoles.AddAsync(userRole);
            }

            await _context.SaveChangesAsync();  // loopdan tashqarida — bir marta saqlaymiz
        }

        public async Task<List<UserRoleDto>> GetListAsync(UserRoleFilter userRoleFilter)
        {

            //return await _context.UserRoles
            //.Where(x => x.UserId == userId)
            //.Select(x => x.Role.Name)
            //.ToListAsync();

            var userRole = _context.UserRoles
                .AsQueryable();

            if (userRoleFilter.UserId != 0)
                userRole = userRole.Where(x => x.UserId == userRoleFilter.UserId);

            if (userRoleFilter.RoleId != 0)
                userRole = userRole.Where(x => x.RoleId == userRoleFilter.RoleId);

            return await userRole.Select(u => new UserRoleDto
            {
                Id = u.Id,
                UserId = u.UserId,
                RoleId = u.RoleId,
            }).ToListAsync();
        }

        public async Task RemoveAsync(int userId, int roleId)
        {
            var userRole = await _context.UserRoles
           .FirstOrDefaultAsync(x => x.UserId == userId && x.RoleId == roleId);

            if (userRole == null)
                throw new Exception("UserRole not found");

            _context.UserRoles.Remove(userRole);
            await _context.SaveChangesAsync();
        }
    }
}
