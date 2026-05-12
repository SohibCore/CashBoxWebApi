using CashBox.Repository.Dtos.UserDtos;
using CashBox.Service.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] UserFilterDto userFilterDto)
        {
            var result = await _userService.GetListAsync(userFilterDto);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var users = await _userService.GetAsync(id);

            if (users == null)
                return NotFound();

            return Ok(users);
        }        

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUserDto createUserDto)
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
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _userService.DeleteAsync(id);
            return Ok();
        }
    }
}
