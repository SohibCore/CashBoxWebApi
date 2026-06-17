using CashBox.Service.Integrations.UzasboServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UzasboController : ControllerBase
    {
        private readonly IUzasboService _service;

        public UzasboController(IUzasboService service)
        {
            _service = service;
        }

        [HttpGet("{tin}")]
        public async Task<IActionResult> Get([FromRoute] string tin)
        {
            var result = await _service.GetFacturaAsync(tin);

            return Ok(result);
        }
    }
}
