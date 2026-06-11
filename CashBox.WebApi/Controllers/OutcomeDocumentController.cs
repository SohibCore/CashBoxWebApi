using CashBox.Repository.Dtos.OutcomeDocumentDtos;
using CashBox.Service.Services.OutcomeDocumentService;
using CashBox.Service.Services.OutcomeDocumentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class OutcomeDocumentController : ControllerBase
    {
        private readonly IOutcomeDocumentService _service;
        public OutcomeDocumentController(IOutcomeDocumentService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] OutcomeDocumentFilterDto outcomeDocumentFilterDto)
        {
            var outcomeDocument = await _service.GetListAsync(outcomeDocumentFilterDto);

            return Ok(outcomeDocument);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] long id)
        {
            var outcomeDocument = await _service.GetAsync(id);
            return Ok(outcomeDocument);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOutcomeDocumentDlDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateOutcomeDocumentDto updateOutcomeDocumentDto)
        {
            await _service.UpdateAsync(updateOutcomeDocumentDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Accept(long id)
        {
            var result = await _service.Accept(id);
                return Ok(result);
        }
        [HttpPatch("{id}")]
        public async Task<IActionResult> NotAccept(long id)
        {
            var result = await _service.NotAccept(id);
            return Ok(result);
        }
    }
}
