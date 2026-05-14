using CashBox.Repository.Dtos.RoleDtos;
using CashBox.Service.Services.NewFolder;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] RoleFilterDto roleFilterDto)
        {
            var roles = await _roleService.GetListAsync(roleFilterDto);
            return Ok(roles);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var role = await _roleService.GetAsync(id);
            return Ok(role);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _roleService.DeleteAsync(id);
            return Ok();
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateRoleDto createRoleDto)
        {
            await _roleService.CreateAsync(createRoleDto);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateRoleDto updateRoleDto)
        {
            await _roleService.UpdateAsync(id, updateRoleDto);
            return Ok();
        }
    }
}
