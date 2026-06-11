namespace CashBox.Repository.Dtos.IncomeDocumentDtos
{
    public class UpdateIncomeDocumentDlDto
    {
        public long Id { get; set; }
        public DateTime DocOn { get; set; }
        public int SupplierId { get; set; }
        public List<IncomeDocumentTableDlDto> Tables { get; set; } = new List<IncomeDocumentTableDlDto>();
    }
}
