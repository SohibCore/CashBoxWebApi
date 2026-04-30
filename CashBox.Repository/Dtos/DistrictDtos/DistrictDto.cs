namespace CashBox.Repository.Dtos.DistrictDtos
{
    public class DistrictDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = null!;
        public string Code { get; set; } = null!;
        public string Region { get; set; } = null!;
        public int CreateUserId { get; set; } 
        public DateTime CreateAt { get; set; }
        public int ModifieredUserId { get; set; }
        public DateTime ModifieredAt { get; set; }
    }
}
