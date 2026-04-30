using CashBox.Repository.Dtos.OrganizationDtos;
using CashBox.Service.Services.OrganizationServices;
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
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var organization = await _organizationService.GetAsync(id); //Service chaqirish
            return Ok(organization);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateOrganizationDto createOrganizationDto)
        {
            await _organizationService.CreateAsync(createOrganizationDto);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateOrganizationDto updateOrganizationDto)
        {
            await _organizationService.UpdateAsync(id, updateOrganizationDto);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _organizationService.DeleteAsync(id);
            return Ok();
        }
    }
}
