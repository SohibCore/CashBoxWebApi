using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Repository.Data;
using RepositoryLayer.Entity;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CashBox.Service.Services.AuthService
{
    public class AuthService : IAuthService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;

        public AuthService(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task RegisterAsync(RegisterDto registerDto)
        {
            var user = new User
            {
                UserName = registerDto.UserName,
                Email = registerDto.Email,
                Password = BCrypt.Net.BCrypt.HashPassword(registerDto.Password),
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<string> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.Password))
                throw new Exception("Invalid credentials");

            return GenerateJwt(user);
        }
        public async Task RefreshAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserName == loginDto.UserName);

            if (user == null)
                throw new Exception("Invalid credentials");

            user.Password = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);
            await _context.SaveChangesAsync();
            return;
        }
        private string GenerateJwt(User user)
        {
            var key = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.UserName),
        };
            var creds = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.UtcNow.AddHours(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
