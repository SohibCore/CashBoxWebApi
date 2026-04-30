using CashBox.Repository.Dtos.ContractorAccount;

namespace CashBox.Service.Services.ConractorAccountServices
{
    public interface IContratorAccountService
    {
        Task<List<ContractorAccountDto>> GetAsync(int id);
        Task CreateAsync(CreateContractorAccountDto createContractorAccountDto);
        Task UpdateAsync(int id, UpdateContractorAccountDto updateContractorAccountDto);
        Task DeleteAsync (int id);
    }
}
