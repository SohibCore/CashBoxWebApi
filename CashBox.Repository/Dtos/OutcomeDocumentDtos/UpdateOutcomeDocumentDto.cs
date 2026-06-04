namespace CashBox.Repository.Dtos.OutcomeDocumentDtos
{
    public class UpdateOutcomeDocumentDto
    {
        public long Id { get; set; }
        public DateTime DocOn { get; set; }
        public int SupplierId { get; set; }
        public decimal DocSum { get; set; }

        public ICollection<OutcomeDocumentTableDlDto> Tables = new List<OutcomeDocumentTableDlDto>();
    }
}
