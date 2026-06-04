using CashBox.Repository.Dtos.OutcomeDocumentDtos;
using CashBox.Service.Services.OutcomeDocumentService;

namespace CashBox.Service.Services.OutcomeDocumentServices
{
    public interface IOutcomeDocumentService
    {
        Task<List<OutcomeDocumentListDto>> GetListAsync(OutcomeDocumentFilterDto filter);
        Task<OutcomeDocumentDto> GetAsync(long id);
        Task<long> CreateAsync(CreateOutcomeDocumentDlDto dto); //long tipdagi qiymat qaytaradi
        Task UpdateAsync(UpdateOutcomeDocumentDto dto);
        Task DeleteAsync(long id);
        Task<long> Accept(int id);
        Task<long> NotAccept(int id);
    }
}
