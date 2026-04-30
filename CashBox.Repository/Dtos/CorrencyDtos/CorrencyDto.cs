namespace CashBox.Repository.Dtos.CorrencyDtos
{
    public class CorrencyDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public string FullName { get; set; } = null!;
        public string ShortName { get; set; } = null!;
    }
}
