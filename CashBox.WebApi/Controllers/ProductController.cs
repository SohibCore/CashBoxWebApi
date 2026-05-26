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
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        [HttpGet]
        public async Task<IActionResult> GetList([FromQuery] ProductFilterDto productFilterDto)
        {
            var products = await _productService.GetListAsync(productFilterDto);
            return Ok(products);
        }
        [HttpGet("{Id}")]
        public async Task<IActionResult> Get([FromRoute] int id)
        {
            var product = await _productService.GetAsync(id);
            return Ok(product);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductDto createProductDto)
        {
            await _productService.CreateAsync(createProductDto);
            return Ok();
        }
        [HttpPut("{Id}")]
        public async Task<IActionResult> Update([FromRoute] int id, UpdateProductDto updateProductDto)
        {
            await _productService.UpdateAsync(id, updateProductDto);
            return Ok();
        }
        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            await _productService.DeleteAsync(id);
            return Ok();
        }
    }
}
