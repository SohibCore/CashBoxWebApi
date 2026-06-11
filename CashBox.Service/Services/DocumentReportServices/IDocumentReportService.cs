using CashBox.Repository.Dtos.DocumentReportDto;
using CashBox.Repository.Dtos.DocumentReportDtos;

namespace CashBox.Service.Services.DocumentReportServices
{
    public interface IDocumentReportService
    {
        Task<List<DocumentReportListDto>> GetListAsync(DocumentReportFilterDto filter);
        Task<DocumentReportDto> GetAsync(long id);
        Task<long> CreateAsync(CreateDocumentReportDto dto);
        Task UpdateAsync(UpdateDocumentReportDlDto dto);
        Task DeleteAsync(long id);
    }
}
