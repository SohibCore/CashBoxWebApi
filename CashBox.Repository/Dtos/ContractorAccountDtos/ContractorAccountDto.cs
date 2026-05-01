namespace CashBox.Repository.Dtos.ContractorAccount
{
    public class ContractorAccountDto
    {
        public int Id { get; set; }
        public string Code { get; set; } = null!;
        public int ContractorId { get; set; }
        public string FullName { get; set; } = null!;
    }
}
