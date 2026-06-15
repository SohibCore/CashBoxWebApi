namespace CashBox.Service.Services.DocumentReportServices
{
    public interface IDocumentReportService
    {
        Task<List<DocumentReportListDto>> GetHeaderReportAsync(DocumentReportFilterDto filter);
    }
}
