using CashBox.Service.Services.DocumentReportServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]/[action]")]
    public class DocumentReportController : ControllerBase
    {
        private readonly IDocumentReportService _service;
        public DocumentReportController(IDocumentReportService service)
        {
            _service = service;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] DocumentReportFilterDto filter)
        {
            var result = await _service.GetHeaderReportAsync(filter);
            return Ok(result);
        }

        //[HttpGet("{id}")]
        //public async Task<IActionResult> Get([FromRoute] long id)
        //{
        //    var result = await _service.GetAsync(id);
        //    return Ok(result);
        //}
        //[HttpPost]
        //public async Task<IActionResult> Create([FromBody] CreateDocumentReportDto dto)
        //{
        //    var result = await _service.CreateAsync(dto);
        //    return Ok(result);
        //}
        //[HttpPut]
        //public async Task<IActionResult> Update([FromBody] UpdateDocumentReportDlDto dto)
        //{
        //    await _service.UpdateAsync(dto);
        //    return Ok();
        //}
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> Delete([FromRoute] long id)
        //{
        //    await _service.DeleteAsync(id);
        //    return Ok();
        //}
    }
}
