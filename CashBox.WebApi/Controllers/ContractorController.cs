using CashBox.Repository.Dtos.ContractorDtos;
using CashBox.Service.Services.ContractorService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ContractorController : ControllerBase
    {
        private readonly IContractorService _contratorService;
        public ContractorController(IContractorService contratorService)
        {
            _contratorService = contratorService;
        }
        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ContractorFilterDto contractorFilterDto)
        {
            var contractor = await _contratorService.GetListAsync(contractorFilterDto);

            return Ok(contractor);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var contractor = await _contratorService.GetAsync(id);
            return Ok(contractor);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateContractorDto createContractorDto)
        {
            await _contratorService.CreateAsync(createContractorDto);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateContractorDto updateContractorDto)
        {
            await _contratorService.UpdateAsync(id, updateContractorDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _contratorService.DeleteAsync(id);
            return Ok();
        }
    }
}
