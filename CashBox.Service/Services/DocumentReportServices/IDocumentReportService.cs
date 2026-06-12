namespace CashBox.Service.Services.DocumentReportServices
{
    public interface IDocumentReportService
    {
        Task<List<DocumentReportListDto>> GetHeaderReportAsync(DocumentReportFilterDto filter);
        //Task<DocumentReportDto> GetAsync(long id);
        //Task<long> CreateAsync(CreateDocumentReportDto dto);
        //Task UpdateAsync(UpdateDocumentReportDlDto dto);
        //Task DeleteAsync(long id);
    }
}
