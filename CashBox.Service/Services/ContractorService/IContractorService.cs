using CashBox.Repository.Dtos.ContractorDtos;

namespace CashBox.Service.Services.ContractorService
{
    public interface IContractorService
    {
        Task<ContractorDto> GetAsync(int id);
        Task<List<ContractorDto>> GetListAsync(ContractorFilterDto contractorFilterDto);
        Task CreateAsync(CreateContractorDto createContractorDto);
        Task UpdateAsync(int id, UpdateContractorDto updateContractorDto);
        Task DeleteAsync(int id);
    }
}
