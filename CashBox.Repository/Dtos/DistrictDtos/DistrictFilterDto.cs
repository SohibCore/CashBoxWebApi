namespace CashBox.Repository.Dtos.DistrictDtos
{
    public class DistrictFilterDto
    {
        public int? Id { get; set; }
        public string? FullName { get; set; }
        public string? Code { get; set; } 
        public string? Region { get; set; } 
        public int? CreateUserId { get; set; }
        public DateTime? CreateAt { get; set; }
        public int? ModifieredUserId { get; set; }
        public DateTime? ModifieredAt { get; set; }
    }
}
