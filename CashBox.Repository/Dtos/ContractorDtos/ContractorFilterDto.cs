namespace CashBox.Repository.Dtos.ContractorDtos
{
    public class ContractorFilterDto
    {
        public int? Id { get; set; }
        public string? Pinfl { get; set; } 
        public string? ShortName { get; set; } 
        public string? FullName { get; set; } 
        public int? RegionId { get; set; }
        public int? DistrictId { get; set; }
    }
}
