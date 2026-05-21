using CashBox.Repository.Dtos.CorrencyDtos;
using CashBox.Service.Services.CorrencyServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CurrencyController : ControllerBase
    {
        private readonly ICurrencyService _currencyService;
        public CurrencyController(ICurrencyService correncyService)
        {
            _currencyService = correncyService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] CurrencyFilterDto currencyFilterDto)
        {
            var currency = await _currencyService.GetListAsync(currencyFilterDto);

            return Ok(currency);
        }
        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var corrency = await _currencyService.GetAsync(id);
            return Ok(corrency);
        }
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateCorrencyDto createCorrencyDto)
        {
            await _currencyService.CreateAsync(createCorrencyDto);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateCorrencyDto updateCorrencyDto)
        {
            await _currencyService.UpdateAsync(id, updateCorrencyDto);
            return Ok();
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _currencyService.DeleteAsync(id);
            return Ok();
        }
    }
}
