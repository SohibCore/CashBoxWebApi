namespace CashBox.Repository.Dtos.UserDtos
{
    public class UserFilterDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Pinfl { get; set; } = null!;
        public int OrganizationId { get; set; } 
        public string PassportSeries { get; set; } = null!;
    }
}
