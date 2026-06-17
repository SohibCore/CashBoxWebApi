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
        public async Task<IActionResult> GetList([FromQuery] IncomeDocumentFilterDto dto)
        {
            var incomeDocuments = await _service.GetListAsync(dto);
            return Ok(incomeDocuments);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var incomeDocument = await _service.GetAsync(id);
            return Ok(incomeDocument);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateIncomeDocumentDlDto dto)
        {
            var incomeDocument = await _service.CreateAsync(dto);
            return Ok(incomeDocument);  
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateIncomeDocumentDlDto dto)
        {
            await _service.UpdateAsync(dto);
            return Ok("Hujjat yangilandi");
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            await _service.DeleteAsync(id);
            return Ok("Hujjat o'chirildi");
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
