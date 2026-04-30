namespace CashBox.Repository.Dtos.RegionDtos
{
    public class RegionDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
        public string Code { get; set; } = null!;
    }
}
