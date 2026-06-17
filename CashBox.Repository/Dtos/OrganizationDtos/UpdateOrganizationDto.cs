namespace CashBox.Repository.Dtos.OrganizationDtos
{
    public class UpdateOrganizationDto
    {
        public string? Inn { get; set; } = null!;
        public string? FullName { get; set; } = null!;
        public string? ShortName { get; set; } = null!;
        public int RegionId { get; set; }
        public int? DistrictId { get; set; } = null!;
    }
}
