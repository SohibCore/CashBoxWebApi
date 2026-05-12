using CashBox.Repository.Dtos.UserDtos;

namespace CashBox.Service.Services.UserServices
{
    public interface IUserService
    {
        Task<UserDto> GetAsync(int id);
        Task<List<UserDto>> GetListAsync(UserFilterDto userFilterDto);
        Task CreateAsync(CreateUserDto createUserDto);
        Task UpdateAsync(int id, UpdateUserDto updateUserDto);
        Task DeleteAsync(int id);
        Task<UserDto> GetMe(int id);
    }
}
