using CashBox.Repository.Dtos.IncomeDocumentDtos;
using CashBox.Service.Services.IncomeDocumentSerives;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class IncomeDocumentController : ControllerBase
    {
        private readonly IIncomeDocumentService _incomeDocumentservice;
        public IncomeDocumentController(IIncomeDocumentService incomeDocumentservice)
        {
            _incomeDocumentservice = incomeDocumentservice;
        }
        //[Authorize(Roles = "Admin,Manager,User")]
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] IncomeDocumentFilterDto incomeDocumentFilterDto)
        {
            var incomeDocuments = await _incomeDocumentservice.GetListAsync(incomeDocumentFilterDto);
            return Ok(incomeDocuments);
        }
        [Authorize(Roles = "Admin,Manager,User")]
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var incomeDocument = await _incomeDocumentservice.GetAsync(id);
            return Ok(incomeDocument);
        }
        //[Authorize(Roles = "Admin,Manager,User")]
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeDocumentDto createIncomeDocumentDto)
        {
            await _incomeDocumentservice.CreateAsync(createIncomeDocumentDto);
            return Ok();
        }
        //[Authorize(Roles = "Admin,Manager,User")]
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateIncomeDocument updateIncomeDocument)
        {
            await _incomeDocumentservice.UpdateAsync(id, updateIncomeDocument);
            return Ok();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _incomeDocumentservice.DeleteAsync(id);
            return Ok();
        }
    }
}
