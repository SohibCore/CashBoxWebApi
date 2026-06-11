using CashBox.Repository.Dtos.IncomeDocumentDtos;

namespace CashBox.Service.Services.IncomeDocumentServices
{
    public interface IIncomeDocumentService
    {
        Task<List<IncomeDocumentListDto>> GetListAsync(IncomeDocumentFilterDto dto);
        Task<IncomeDocumentDto> GetAsync(long id);
        Task<long> CreateAsync(CreateIncomeDocumentDlDto dto);
        Task UpdateAsync(UpdateIncomeDocumentDlDto dto);
        Task DeleteAsync(long id);
        Task<long> Accept(long id);
        Task<long> NotAccept(long id);
    }
}
