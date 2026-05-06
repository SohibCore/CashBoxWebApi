using CashBox.Repository.Dtos.ContractorDtos;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using RepositoryLayer.Entity;

namespace CashBox.Service.Services.ContractorService
{
    public class ContractorService : IContractorService
    {
        private readonly AppDbContext _context;
        public ContractorService(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(CreateContractorDto createContractorDto)
        {
            var contractor = new Contractor()
            {
                Inn = createContractorDto.Inn,
                Pinfl = createContractorDto.Pinfl,
                ShortName = createContractorDto.ShortName,
                FullName = createContractorDto.FullName,
                RegionId = createContractorDto.RegionId,
                DistrictId = createContractorDto.DistrictId
            };
            await _context.Contractors.AddAsync(contractor);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var contractor = await _context.Contractors.FindAsync(id);

            if (contractor == null)
                throw new Exception($"{id} topilmadi");

            _context.Contractors.Remove(contractor);
            await _context.SaveChangesAsync();
        }
        public async Task<List<ContractorDto>> GetAsync(int id)
        {
            var contractor = await _context.Contractors.FindAsync(id);
            return new List<ContractorDto>()
            {
                new ContractorDto()
                {
                    Id = contractor.Id,
                    Pinfl = contractor.Pinfl,
                    ShortName = contractor.ShortName,
                    FullName = contractor.FullName,
                    RegionId = contractor.RegionId,
                    DistrictId = contractor.DistrictId
                }
            };
        }

        public async Task<List<ContractorDto>> GetListAsync(ContractorFilterDto contractorFilterDto)
        {
            var contractor = _context.Contractors.AsQueryable();

            if (!string.IsNullOrWhiteSpace(contractorFilterDto.Pinfl))
                contractor = contractor.Where(x => x.Pinfl.ToLower().Contains(contractorFilterDto.Pinfl));
            if (!string.IsNullOrWhiteSpace(contractorFilterDto.ShortName))
                contractor = contractor.Where(x => x.ShortName.ToLower().Contains(contractorFilterDto.ShortName));
            if (!string.IsNullOrWhiteSpace(contractorFilterDto.FullName))
                contractor = contractor.Where(x => x.FullName.ToLower().Contains(contractorFilterDto.FullName));
            if (contractorFilterDto.Id != 0 && contractorFilterDto.Id != null)
                contractor = contractor.Where(x => x.Id == contractorFilterDto.Id);
            if (contractorFilterDto.RegionId != 0 && contractorFilterDto.RegionId != null)
                contractor = contractor.Where(x => x.RegionId == contractorFilterDto.RegionId);
            if (contractorFilterDto.DistrictId != 0 && contractorFilterDto.DistrictId != null)
                contractor = contractor.Where(x => x.DistrictId == contractorFilterDto.DistrictId);

            return await contractor.Select(u => new ContractorDto
            {
                Id = u.Id,
                Pinfl = u.Pinfl,
                ShortName = u.ShortName,
                FullName = u.FullName,
                RegionId = u.RegionId,
                DistrictId = u.DistrictId
            }).ToListAsync();
        }
        public async Task UpdateAsync(int id, UpdateContractorDto updateContractorDto)
        {
            var contractor = await _context.Contractors.FindAsync(id);

#pragma warning disable CS8601 // Possible null reference assignment.
            contractor.Inn = updateContractorDto.Inn;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            contractor.Pinfl = updateContractorDto.Pinfl;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            contractor.ShortName = updateContractorDto.ShortName;
#pragma warning restore CS8601 // Possible null reference assignment.
#pragma warning disable CS8601 // Possible null reference assignment.
            contractor.FullName = updateContractorDto.FullName;
#pragma warning restore CS8601 // Possible null reference assignment.
        }
    }
}
