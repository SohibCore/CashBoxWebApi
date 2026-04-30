using CashBox.Repository.Dtos.CorrencyRateDtos;
using CashBox.Service.Services.CorrencyRateServices;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CorrencyRateController : ControllerBase
    {
        private readonly ICurrencyRateService _correncyRateService;
        public CorrencyRateController(ICurrencyRateService correncyRateService)
        {
            _correncyRateService = correncyRateService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var correncyRate = await _correncyRateService.GetAsync(id);
            return Ok(correncyRate);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCurrencyRateDto createCorrencyRateDto)
        {
            await _correncyRateService.CreateAsync(createCorrencyRateDto);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateCurrencyRateDto updateCorrencyRateDto)
        {
            await _correncyRateService.UpdateAsync(id, updateCorrencyRateDto);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _correncyRateService.DeleteAsync(id);
            return Ok();
        }
    }
}
