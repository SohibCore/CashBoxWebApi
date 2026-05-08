using System.ComponentModel.DataAnnotations;

namespace CashBox.Service.Services.AuthService
{
    public class RegisterDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Email { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
