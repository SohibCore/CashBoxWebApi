using CashBox.Repository.Dtos.IncomeDocumentDtos;

namespace CashBox.Service.Services.IncomeDocumentServices
{
    public interface IIncomeDocumentService
    {
        Task<List<IncomeDocumentListDto>> GetListAsync(IncomeDocumentFilterDto incomeDocumentFilterDto);
        Task<IncomeDocumentDto> GetAsync(long id);
        Task<long> CreateAsync(CreateIncomeDocumentDlDto dto);
        Task UpdateAsync(UpdateIncomeDocumentDlDto dto);
        Task DeleteAsync(long id);
        Task<long> Accept(int id);
        Task<long> NotAccept(int id);
    }
}
