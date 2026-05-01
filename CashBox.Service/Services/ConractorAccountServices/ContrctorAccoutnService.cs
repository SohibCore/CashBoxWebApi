using CashBox.Repository.Dtos.ContractorAccount;
using CashBox.Repository.Dtos.ContractorAccountDtos;
using Microsoft.EntityFrameworkCore;
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
            if (contractor == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            _context.ContractorAccounts.Remove(contractor);
            await _context.SaveChangesAsync();
        }

        public async Task<ContractorAccountDto> GetAsync(int id)
        {
            var contractor = await _context.ContractorAccounts.FindAsync(id);

            if (contractor == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new ContractorAccountDto
            {
                Id = contractor.Id,
                Code = contractor.Code,
                FullName = contractor.FullName
            };
        }

        public async Task<List<ContractorAccountDto>> GetListAsync(ContractorAccountFilterDto contractorAccountFilterDto)
        {
            var contractorAccount = _context.ContractorAccounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(contractorAccountFilterDto.FullName))
                contractorAccount = contractorAccount.Where(x => x.FullName.ToLower().Contains(contractorAccountFilterDto.FullName.ToLower()));
            if (!string.IsNullOrWhiteSpace(contractorAccountFilterDto.Code))
                contractorAccount = contractorAccount.Where(x => x.Code.ToLower().Contains(contractorAccountFilterDto.Code.ToLower()));
            if (contractorAccountFilterDto.Id != 0 && contractorAccountFilterDto.Id != null)
                contractorAccount = contractorAccount.Where(x => x.Id == contractorAccountFilterDto.Id);

            return await contractorAccount.Select(u => new ContractorAccountDto
            {
                Id = u.Id,
                Code = u.Code,
                FullName = u.FullName
            }).ToListAsync();
        }

        public async Task UpdateAsync(int id, UpdateContractorAccountDto updateContractorAccountDto)
        {
            var contractor = _context.ContractorAccounts.Find(id);
            if (contractor == null)
                throw new KeyNotFoundException($"{id} topilmadi");

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
