using CashBox.Repository.Dtos.IncomeDocumentDtos;

namespace CashBox.Service.Services.IncomeDocumentSerives
{
    public interface IIncomeDocumentService
    {
        Task<List<IncomeDocumentDto>> GetListAsync(IncomeDocumentFilterDto incomeDocumentFilterDto);
        Task<IncomeDocumentDto> GetAsync(int id);
        Task CreateAsync(CreateIncomeDocumentDto createIncomeDocumentDto);
        Task UpdateAsync(int id, UpdateIncomeDocument updateIncomeDocument);
        Task DeleteAsync(int id);
    }
}
