using System.ComponentModel.DataAnnotations;

namespace CashBox.Service.Services.UserServices
{
    public class UserLoginDto
    {
        [Required]
        public string UserName { get; set; } = null!;

        [Required]
        public string Password { get; set; } = null!;
    }
}
