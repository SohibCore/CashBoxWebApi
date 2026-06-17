using CashBox.Repository.Dtos.OrganizationDtos;
using CashBox.Service.Services.OrganizationServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OrganizationController : ControllerBase
    {
        private readonly IOrganizationService _service;
        public OrganizationController(IOrganizationService service)
        {
            _service = service;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] OrganizationFilterDto dto)
        {
            var organizations = await _service.GetListAsync(dto);
            return Ok(organizations);
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetListForRegister([FromQuery] OrganizationFilterDto dto)
        {
            var organization = await _service.GetListAsync(dto);
            return Ok(organization);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var organization = await _service.GetAsync(id); 
            return Ok(organization);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok("Tashilot yaratildi");
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateOrganizationDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok("Tashkilot yangilandi");
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return Ok("Tashkilot o'chirildi");
        }
    }
}
