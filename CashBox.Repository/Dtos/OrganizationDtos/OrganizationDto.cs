namespace CashBox.Repository.Dtos.OrganizationDtos
{
    public class OrganizationDto
    {
        public int Id { get; set; }
        public string Inn { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public int RegionId { get; set; }
        public string District { get; set; } = null!;
    }
}
