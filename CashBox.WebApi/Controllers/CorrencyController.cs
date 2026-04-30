using CashBox.Repository.Dtos.CorrencyDtos;
using CashBox.Service.Services.CorrencyServices;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class CorrencyController : ControllerBase
    {
        private readonly ICorrencyService _currencyService;
        public CorrencyController(ICorrencyService correncyService)
        {
            _currencyService = correncyService;
        }
        [HttpGet]
        public async Task<IActionResult> Get(int id)
        {
            var corrency = await _currencyService.GetAsync(id);
            return Ok(corrency);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateCorrencyDto createCorrencyDto)
        {
            await _currencyService.CreateAsync(createCorrencyDto);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update(int id, UpdateCorrencyDto updateCorrencyDto)
        {
            await _currencyService.UpdateAsync(id, updateCorrencyDto);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _currencyService.DeleteAsync(id);
            return Ok();
        }
    }
}
