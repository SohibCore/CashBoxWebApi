using CashBox.Repository.Dtos.SupplierDtos;
using CashBox.Service.Services.SupplierServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class SupplierController : ControllerBase
    {
        private readonly ISupplierService _supplierService;

        public SupplierController(ISupplierService supplierService)
        {
            _supplierService = supplierService;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] SupplierFilterDto supplierFilterDto)
        {
            var suppliers = await _supplierService.GetListAsync(supplierFilterDto);

            return Ok(suppliers);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var supplier = await _supplierService.GetAsync(id);

            if (supplier == null)
                throw new KeyNotFoundException();

            return Ok(supplier);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSupplierDto createSupplierDto)
        {
            await _supplierService.CreateAsync(createSupplierDto);
            return Ok();
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateSupplierDto updateSupplierDto)
        {
            await _supplierService.UpdateAsync(id, updateSupplierDto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _supplierService.DeleteAsync(id);
            return Ok();
        }
    }
}
