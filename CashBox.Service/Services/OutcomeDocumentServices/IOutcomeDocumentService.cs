using CashBox.Repository.Dtos.OutcomeDocumentDtos;

namespace CashBox.Service.Services.OutcomeDocumentService
{
    public interface IOutcomeDocumentService
    {
        Task<List<OutcomeDocumentDto>> GetListAsync(OutcomeDocumentFilterDto outcomeDocumentFilterDto);
        Task<OutcomeDocumentDto> GetAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, UpdateOutcomeDocumentDto updateOutcomeDocumentDto);
        Task CreateAsync(CreateOutcomeDocumentDto createOutcomeDocumentDto);
    }
}
