using CashBox.Service.Services.AuthService;
using CashBox.Service.Services.UserServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CashBox.WebApi.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto registerDto)
        {
            await _authService.RegisterAsync(registerDto);
            return Ok();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto loginDto)
        {
            var token = await _authService.LoginAsync(loginDto);
            return Ok(new { token });
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh(LoginDto loginDto)
        {
            await _authService.RefreshAsync(loginDto);
            return Ok();
        }
        [Authorize]
        [HttpGet("getme")]
        public async Task<IActionResult> GetMe()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var users = await _userService.GetMe(userId);

            if (users == null)
                return NotFound();

            return Ok(users);
        }
    }
}
