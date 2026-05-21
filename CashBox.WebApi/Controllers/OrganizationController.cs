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
        private readonly IOrganizationService _organizationService;
        public OrganizationController(IOrganizationService organizationService)
        {
            _organizationService = organizationService;
        }
        [AllowAnonymous]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] OrganizationFilterDto organizationFilterDto)
        {
            var organization = await _organizationService.GetListAsync(organizationFilterDto);

            return Ok(organization);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var organization = await _organizationService.GetAsync(id); //Service chaqirish
            return Ok(organization);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOrganizationDto createOrganizationDto)
        {
            await _organizationService.CreateAsync(createOrganizationDto);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateOrganizationDto updateOrganizationDto)
        {
            await _organizationService.UpdateAsync(id, updateOrganizationDto);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _organizationService.DeleteAsync(id);
            return Ok();
        }
    }
}
