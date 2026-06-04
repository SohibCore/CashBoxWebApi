using CashBox.Repository.Dtos.OutcomeDocumentDtos;
using CashBox.Service.Services.OutcomeDocumentService;
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
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var outcomeDocument = await _service.GetAsync(id);
            return Ok(outcomeDocument);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateOutcomeDocumentDto createOutcomeDocumentDto)
        {
            await _service.CreateAsync(createOutcomeDocumentDto);
            return Ok();
        }
        [HttpPut]
        public async Task<IActionResult> Update([FromQuery] int id, [FromBody] UpdateOutcomeDocumentDto updateOutcomeDocumentDto)
        {
            await _service.UpdateAsync(id, updateOutcomeDocumentDto);
            return Ok();
        }
        [HttpDelete]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
