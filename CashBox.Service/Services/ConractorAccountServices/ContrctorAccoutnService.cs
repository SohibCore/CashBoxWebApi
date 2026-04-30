using CashBox.Repository.Dtos.ContractorAccount;
using Repository.Data;
using RepositoryLayer.Entity;

namespace CashBox.Service.Services.ConractorAccountServices
{
    public class ContratorAccountService : IContratorAccountService
    {
        private readonly AppDbContext _context;
        public ContratorAccountService(AppDbContext appDbContext)
        {
            _context = appDbContext;
        }
        public async Task CreateAsync(CreateContractorAccountDto createContractorAccountDto)
        {
            var corrency = new ContractorAccount()
            {
                Code = createContractorAccountDto.Code,
                ContractorId = createContractorAccountDto.ContractorId,
                FullName = createContractorAccountDto.FullName
            };
            await _context.ContractorAccounts.AddAsync(corrency);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var contractor = await _context.ContractorAccounts.FindAsync(id);
            if (contractor != null)
            {
                _context.ContractorAccounts.Remove(contractor);
                await _context.SaveChangesAsync();
            }
            else
            {
                throw new Exception("O'chirib bo'lmadi");
            }
        }

        public async Task<List<ContractorAccountDto>> GetAsync(int id)
        {
            var contractor = _context.ContractorAccounts.Find(id);
            if (contractor == null)
                throw new Exception("Error");

            return new List<ContractorAccountDto>
            {
                new ContractorAccountDto
                {
                    Id = contractor.Id,
                    Code = contractor.Code,
                    FullName = contractor.FullName
                }
            };
        }

        public async Task UpdateAsync(int id, UpdateContractorAccountDto updateContractorAccountDto)
        {
            var contractor = _context.ContractorAccounts.Find(id);
            if (contractor == null)
                throw new Exception($"{id} topilmadi");

#pragma warning disable CS8601 // Possible null reference assignment.
            contractor.Code = updateContractorAccountDto.Code;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            contractor.FullName = updateContractorAccountDto.FullName;
#pragma warning restore CS8601 // Possible null reference assignment.
            await _context.SaveChangesAsync();
        }
    }
}
