using CashBox.Repository.Dtos.UserDtos;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using RepositoryLayer.Entity;

namespace CashBox.Service.Services.UserServices
{

    public class UserService : IUserService
    {
        private readonly AppDbContext _context;
        public UserService(AppDbContext context) // Dependency Injection orqali AppDbContext ni qabul qilamiz
        {
            _context = context;
        }
        public async Task<UserDto> GetAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new UserDto
            {
                Id = user.Id,
                UserName = user.UserName,
                FullName = user.FullName,
                ShortName = user.ShortName,
                Pinfl = user.Pinfl,
                PhoneNumber = user.PhoneNumber,
                Address = user.Address,
                OrganizationId = user.OrganizationId,
                DateOfBirth = user.DateOfBirth,
                PassportSeries = user.PassportSeries,
                Email = user.Email
            };
        }
        public async Task CreateAsync(CreateUserDto createUserDto)
        {
            var user = new User
            {
                UserName = createUserDto.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(createUserDto.Password),
                FullName = createUserDto.FullName,
                ShortName = createUserDto.ShortName,
                Pinfl = createUserDto.Pinfl,
                PhoneNumber = createUserDto.PhoneNumber,
                Address = createUserDto.Address,
                OrganizationId = createUserDto.OrganizationId,
                DateOfBirth = createUserDto.DateOfBirth,
                PassportSeries = createUserDto.PassportSeries,
                Email = createUserDto.Email
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(int id, UpdateUserDto updateUserDto)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            if (updateUserDto.UserName != null)
                user.UserName = updateUserDto.UserName; //Bazadagi UserNameni frontdan kelgan yangi dataga o'zgartirish

            if (updateUserDto.FullName != null)
                user.FullName = updateUserDto.FullName;

            if (updateUserDto.Password != null)
                user.Password = updateUserDto.Password;

            if (updateUserDto.ShortName != null)
                user.ShortName = updateUserDto.ShortName;

            if (updateUserDto.Pinfl != null)
                user.Pinfl = updateUserDto.Pinfl;

            if (updateUserDto.PhoneNumber != null)
                user.PhoneNumber = updateUserDto.PhoneNumber;

            if (updateUserDto.Address != null)
                user.Address = updateUserDto.Address;

            //if (updateUserDto.OrganizationId != 0)
            //    user.OrganizationId = updateUserDto.OrganizationId;

            if (updateUserDto.DateOfBirth != null)
                user.DateOfBirth = updateUserDto.DateOfBirth;

            if (updateUserDto.PassportSeries != null)
                user.PassportSeries = updateUserDto.PassportSeries;

            if (updateUserDto.Email != null)
                user.Email = updateUserDto.Email;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
        }

        public async Task<UserDto> GetMe(int id)
        {
            return await GetAsync(id);
        }

        public async Task<List<UserDto>> GetListAsync(UserFilterDto userFilterDto)
        {
            var user = _context.Users.AsQueryable();

            if (userFilterDto.Id != 0)
                user = user.Where(x => x.Id == userFilterDto.Id);
            if(userFilterDto.OrganizationId != 0)
                user = user.Where(x => x.OrganizationId == userFilterDto.OrganizationId);
            if (!string.IsNullOrWhiteSpace(userFilterDto.UserName))
                user = user.Where(x => x.UserName.Contains(userFilterDto.UserName));
            if (!string.IsNullOrWhiteSpace(userFilterDto.FullName))
                user = user.Where(x => x.FullName.Contains(userFilterDto.FullName));
            if (!string.IsNullOrWhiteSpace(userFilterDto.ShortName))
                user = user.Where(x => x.ShortName.Contains(userFilterDto.ShortName));
            if (!string.IsNullOrWhiteSpace(userFilterDto.Pinfl))
                user = user.Where(x => x.Pinfl.Contains(userFilterDto.Pinfl));
            if (!string.IsNullOrWhiteSpace(userFilterDto.PassportSeries))
                user = user.Where(x => x.PassportSeries.Contains(userFilterDto.PassportSeries));
            if (!string.IsNullOrWhiteSpace(userFilterDto.PhoneNumber))
                user = user.Where(x => x.PhoneNumber.Contains(userFilterDto.PhoneNumber));
            if (!string.IsNullOrWhiteSpace(userFilterDto.DateOfBirth))
                user = user.Where(x => x.UserName.Contains(userFilterDto.DateOfBirth));
            if (!string.IsNullOrWhiteSpace(userFilterDto.Address))
                user = user.Where(x => x.Address.Contains(userFilterDto.Address));


            return await user.Select(u => new UserDto
            {
                Id = u.Id,
                UserName = u.UserName,
                FullName = u.FullName,
                ShortName = u.ShortName,
                Pinfl = u.Pinfl,
                PhoneNumber = u.PhoneNumber,
                Address = u.Address,
                OrganizationId = u.OrganizationId,
                DateOfBirth = u.DateOfBirth,
                PassportSeries = u.PassportSeries
            }).ToListAsync();
        }

    }
}
