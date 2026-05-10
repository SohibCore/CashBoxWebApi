using System.ComponentModel.DataAnnotations;

namespace CashBox.Repository.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public string? UserName { get; set; }

        [Required]
        public string? FullName { get; set; }

        [Required]
        public string? ShortName { get; set; } 

        [Required]
        [StringLength(14)]
        public string? Pinfl { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        public string? Email { get; set; }
        public int OrganizationId { get; set; }

        [Required]
        [StringLength(10)]
        public string? DateOfBirth { get; set; }

        [Required]
        public string? PassportSeries { get; set; } 
        public string? Password { get; set; } 
    }
}
