using CashBox.Repository.Dtos.UserRoleDtos;
using CashBox.Service.Services.UserRoleService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserRoleController : ControllerBase
    {
        private readonly IUserRoleService _service;

        public UserRoleController(IUserRoleService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> AssignRole([FromQuery]  int userId, [FromBody] List<int> roleIds)
        {
            await _service.AssignAsync(userId, roleIds);
            return Ok("Rollar biriktirildi");
        }

        [HttpDelete]
        public async Task<IActionResult> Remove(int userId, int roleId)
        {
            await _service.RemoveAsync(userId, roleId);
            return Ok("Role removed");
        }
        [HttpGet]
        public async Task<IActionResult> GetList(UserRoleFilter userRoleFilter)
        {
            var roles = await _service.GetListAsync(userRoleFilter);
            return Ok(roles);
        }
    }
}
