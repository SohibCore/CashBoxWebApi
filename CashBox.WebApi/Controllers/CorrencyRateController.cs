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
        public async Task<IActionResult> GetList([FromQuery] CurrencyRateFilterDto currencyRateFilterDto)
        {
            var currencyRate = await _correncyRateService.GetListAsync(currencyRateFilterDto);

            return Ok(currencyRate);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var correncyRate = await _correncyRateService.GetAsync(id);
            return Ok(correncyRate);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCurrencyRateDto createCorrencyRateDto)
        {
            await _correncyRateService.CreateAsync(createCorrencyRateDto);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCurrencyRateDto updateCorrencyRateDto)
        {
            await _correncyRateService.UpdateAsync(id, updateCorrencyRateDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _correncyRateService.DeleteAsync(id);
            return Ok();
        }
    }
}
