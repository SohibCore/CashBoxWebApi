namespace CashBox.Repository.Dtos.ContractorDtos
{
    public class ContractorDto
    {
        public int Id { get; set; }
        public string Pinfl { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public int RegionId { get; set; }
        public int DistrictId { get; set; }
    }
}
