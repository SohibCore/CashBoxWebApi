namespace CashBox.Repository.Dtos.OrganizationDtos
{
    public class OrganizationFilterDto
    {
        public int? Id { get; set; }
        public string? Inn { get; set; } 
        public string? FullName { get; set; } 
        public string? ShortName { get; set; } 
        public int? RegionId { get; set; }
        public string? District { get; set; }
    }
}
