namespace CashBox.Repository.Dtos.DistrictDtos
{
    public class DistrictDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Region { get; set; } = null!;
    }
}
