using CashBox.Repository.Dtos.ContractorAccount;
using CashBox.Repository.Dtos.ContractorAccountDtos;
using CashBox.Service.Services.ConractorAccountServices;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ContractorAccountController : ControllerBase
    {
        private readonly IContratorAccountService _contratorAccountService;
        public ContractorAccountController(IContratorAccountService contratorAccountService)
        {
            _contratorAccountService = contratorAccountService;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ContractorAccountFilterDto contractorAccountFilterDto)
        {
            var contractor = await _contratorAccountService.GetListAsync(contractorAccountFilterDto);

            return Ok(contractor);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var contractor = await _contratorAccountService.GetAsync(id);
            return Ok(contractor);
        }
        [HttpPost]
        public async Task<IActionResult> Create(CreateContractorAccountDto createContractorAccountDto)
        {
            await _contratorAccountService.CreateAsync(createContractorAccountDto);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateContractorAccountDto updateContractorAccountDto)
        {
            await _contratorAccountService.UpdateAsync(id, updateContractorAccountDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _contratorAccountService.DeleteAsync(id);
            return Ok();
        }
    }
}
