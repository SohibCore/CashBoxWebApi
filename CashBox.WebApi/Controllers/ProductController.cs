using CashBox.Repository.Dtos.ProductDtos;
using CashBox.Service.Services.ProductServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CashBox.WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _service;
        public ProductController(IProductService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ProductFilterDto dto)
        {
            var products = await _service.GetListAsync(dto);
            return Ok(products);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var product = await _service.GetAsync(id);
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            await _service.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateProductDto dto)
        {
            await _service.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _service.DeleteAsync(id);
            return Ok();
        }
    }
}
