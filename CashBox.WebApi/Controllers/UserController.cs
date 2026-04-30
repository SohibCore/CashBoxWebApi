using Microsoft.AspNetCore.Mvc;
using CashBox.Service.Services.UserServices;
using CashBox.Repository.Dtos.UserDtos;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> GetList([FromBody] UserFilterDto userFilterDto)
        {
            var result = await _userService.GetListAsync(userFilterDto);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var users = await _userService.GetAsync(id);

            if (users == null)
                return NotFound();

            return Ok(users);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateUserDto createUserDto)
        {
            await _userService.CreateAsync(createUserDto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserDto updateUserDto)
        {   
            await _userService.UpdateAsync(id, updateUserDto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
    }
}
