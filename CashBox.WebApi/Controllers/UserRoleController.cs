using CashBox.Repository.Dtos.UserRoleDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Services.UserRoleService;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _userRoleService;
        public UserRoleController(IUserRoleService userRoleService)
        {
            _userRoleService = userRoleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] UserRoleFilter userRoleFilter)
        {
            var result = await _userRoleService.GetListAsync(userRoleFilter);
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var userRole = await _userRoleService.GetAsync(id);

            if(userRole == null)
                return NotFound();

            return Ok(userRole);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] UserRole userRole)
        {
            await _userRoleService.CreateAsync(userRole);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userRoleService.DeleteAsync(id);
            return Ok();
        }
    }
}
