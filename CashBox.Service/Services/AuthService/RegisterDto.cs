using System.ComponentModel.DataAnnotations;

namespace CashBox.Service.Services.AuthService
{
    public class RegisterDto
    {
        [Required]
        [StringLength(300)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string Password { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string FullName { get; set; } = null!;

        [Required]
        [StringLength(300)]
        public string ShortName { get; set; } = null!;

        [Required]
        [StringLength(14)]
        public string Pinfl { get; set; } = null!;

        [Required]
        [StringLength(9)]
        public string PhoneNumber { get; set; } = null!;

        [Required]
        public string Address { get; set; } = null!;

        [Required]
        public int OrganizationId { get; set; }

        [Required]
        [StringLength(10)]
        public string DateOfBirth { get; set; } = null!;

        [Required]
        [StringLength(9)]
        public string PassportSeries { get; set; } = null!;
    }
}
