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
        public async Task<IActionResult> GetList([FromQuery] DistrictFilterDto districtFilterDto)
        {
            var district = await _districtService.GetListAsync(districtFilterDto);

            return Ok(district);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var district = await _districtService.GetAsync(id);
            return Ok(district);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDistrictDto createDistrictDto)
        {
            await _districtService.CreateAsync(createDistrictDto);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateDistrictDto updateDistrictDto)
        {
            await _districtService.UpdateAsync(id, updateDistrictDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _districtService.DeleteAsync(id);
            return Ok();
        }
    }
}
