using CashBox.Repository.Dtos.ContractorAccount;
using CashBox.Repository.Dtos.ContractorAccountDtos;

namespace CashBox.Service.Services.ConractorAccountServices
{
    public interface IContratorAccountService
    {
        Task<ContractorAccountDto> GetAsync(int id);
        Task<List<ContractorAccountDto>> GetListAsync(ContractorAccountFilterDto contractorAccountFilterDto);
        Task CreateAsync(CreateContractorAccountDto createContractorAccountDto);
        Task UpdateAsync(int id, UpdateContractorAccountDto updateContractorAccountDto);
        Task DeleteAsync(int id);
    }
}
