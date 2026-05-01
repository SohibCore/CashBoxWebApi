using CashBox.Repository.Dtos.RegionDtos;
using CashBox.Service.Services.RegionServices;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class RegionController : ControllerBase
    {
        private readonly IRegionService _regionService;
        public RegionController(IRegionService regionService)
        {
            _regionService = regionService;
        }
        [HttpPost]
        public async Task<IActionResult> GetList(string searchRegion, RegionFilterDto regionFilterDto)
        {
            var regions = await _regionService.GetListAsync(searchRegion, regionFilterDto);
            return Ok(regions);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var region = await _regionService.GetAsync(id);
            return Ok(region);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateRegionDto createRegionDto)
        {
            await _regionService.CreateAsync(createRegionDto);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateRegionDto updateRegionDto)
        {
            await _regionService.UpdateAsync(id, updateRegionDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _regionService.DeleteAsync(id);
            return Ok();
        }
    }
}
