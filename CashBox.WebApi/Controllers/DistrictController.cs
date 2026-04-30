using CashBox.Repository.Dtos.DistrictDtos;
using CashBox.Service.Services.DistrictServices;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class DistrictController : ControllerBase
    {
        private readonly IDistrictService _districtService;

        public DistrictController(IDistrictService districtService)
        {
            _districtService = districtService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var district = await _districtService.GetAsync(id);
            return Ok(district);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateDistrictDto createDistrictDto)
        {
            await _districtService.CreateAsync(createDistrictDto);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateDistrictDto updateDistrictDto)
        {
            await _districtService.UpdateAsync(id, updateDistrictDto);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _districtService.DeleteAsync(id);
            return Ok();
        }
    }
}
