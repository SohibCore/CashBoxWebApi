using CashBox.Repository.Dtos.IncomeDocumentDtos;
using CashBox.Service.Services.IncomeDocumentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class IncomeDocumentController : ControllerBase
    {
        private readonly IIncomeDocumentService _service;
        public IncomeDocumentController(IIncomeDocumentService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] IncomeDocumentFilterDto incomeDocumentFilterDto)
        {
            var incomeDocuments = await _service.GetListAsync(incomeDocumentFilterDto);
            return Ok(incomeDocuments);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var incomeDocument = await _service.GetAsync(id);
            return Ok(incomeDocument);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeDocumentDlDto createIncomeDocumentDto)
        {
            var id = await _service.CreateAsync(createIncomeDocumentDto);
            return Ok(id);  
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateIncomeDocumentDlDto updateIncomeDocument)
        {
            await _service.UpdateAsync(updateIncomeDocument);
            return Ok();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> Accept([FromRoute] long id)
        {
            var result = await _service.Accept(id);
            return Ok(result);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> NotAccept([FromRoute] long id)
        {
            var result = await _service.NotAccept(id);
            return Ok(result);
        }
    }
}
