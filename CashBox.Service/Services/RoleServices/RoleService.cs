using CashBox.Repository.Dtos.RoleDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Services.NewFolder;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.RoleServices
{
    public class RoleService : IRoleService
    {
        private readonly AppDbContext _context;
        public RoleService(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(CreateRoleDto createRoleDto)
        {
            var role = new Role()
            {
                Name = createRoleDto.Name,
                Description = createRoleDto.Description
            };
            await _context.Roles.AddAsync(role);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();
        }

        public async Task<Role> GetAsync(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new Role
            {
                Id = role.Id,
                Name = role.Name,
                Description = role.Description
            };
        }

        public Task<List<RoleDto>> GetListAsync(RoleFilterDto roleFilterDto)
        {
            var role = _context.Roles.AsQueryable();

            if (roleFilterDto.Id != 0)
                role = role.Where(r => r.Id == roleFilterDto.Id);

            if (!string.IsNullOrEmpty(roleFilterDto.Name))
                role = role.Where(r => r.Name.Contains(roleFilterDto.Name));

            return role.Select(r => new RoleDto
            {
                Id = r.Id,
                Name = r.Name,
                Description = r.Description
            }).ToListAsync();
        }

        public async Task UpdateAsync(int id, UpdateRoleDto updateRoleDto)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            role.Name = updateRoleDto.Name;
            role.Description = updateRoleDto.Description;

            await _context.SaveChangesAsync();
        }
    }
}
