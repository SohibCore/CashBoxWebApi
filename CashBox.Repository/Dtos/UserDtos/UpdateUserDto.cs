namespace CashBox.Repository.Dtos.UserDtos
{
    public class UpdateUserDto
    {
        public string? UserName { get; set; }
        public string? FullName { get; set; }
        public string? ShortName { get; set; } 
        public string? Pinfl { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public string? Email { get; set; }
        public int OrganizationId { get; set; }
        public string? DateOfBirth { get; set; }
        public string? PassportSeries { get; set; } 
        public string? Password { get; set; }
    }
}
