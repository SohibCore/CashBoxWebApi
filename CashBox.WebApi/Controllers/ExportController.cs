using CashBox.Service.Services.ExelServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExportController : ControllerBase
    {
        private readonly IExelService _service;
        private readonly AppDbContext _context;

        public ExportController(IExelService service, AppDbContext context)
        {
            _service = service;
            _context = context;
        }

        [HttpGet("products")]
        public async Task<IActionResult> Products()
        {
            var data = await _context.Products.ToListAsync();

            var bytes = _service.ExportProducts(data);

            return File(
                bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Mahsulotlar.xlsx");
        }
        [HttpGet("users")]
        public async Task<IActionResult> Users()
        {
            var data = await _context.Users.ToListAsync();

            var bytes = _service.ExportUsers(data);

            return File(
                bytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
            "Foydalanuvchilar.xlsx");
        }
    }
}
