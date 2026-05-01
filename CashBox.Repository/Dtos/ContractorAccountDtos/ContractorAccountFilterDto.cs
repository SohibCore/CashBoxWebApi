namespace CashBox.Repository.Dtos.ContractorAccountDtos
{
    public class ContractorAccountFilterDto
    {
        public int? Id { get; set; }
        public string? Code { get; set; }
        public int? ContractorId { get; set; }
        public string? FullName { get; set; }
    }
}
