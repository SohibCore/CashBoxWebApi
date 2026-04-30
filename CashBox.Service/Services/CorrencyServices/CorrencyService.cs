using CashBox.Repository.Dtos.CorrencyDtos;
using Repository.Data;
using Repository.Entity;

namespace CashBox.Service.Services.CorrencyServices
{
    public class CorrencyService : ICorrencyService
    {
        private readonly AppDbContext _context;
        public CorrencyService(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateAsync(CreateCorrencyDto createCorrencyDto)
        {
            var corrency = new Currency
            {
                Code = createCorrencyDto.Code,
                FullName = createCorrencyDto.FullName,
                ShortName = createCorrencyDto.ShortName,
            };
            await _context.Correncys.AddAsync(corrency);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var corrency = await _context.Correncys.FindAsync(id);

            if (corrency == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            _context.Correncys.Remove(corrency);
            await _context.SaveChangesAsync();
        }

        public async Task<CorrencyDto> GetAsync(int id)
        {
            var corrency =await _context.Correncys.FindAsync(id);

            if (corrency == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new CorrencyDto
            {
                Id = corrency.Id,
                FullName = corrency.FullName,
                ShortName = corrency.ShortName,
                Code = corrency.Code
            };
        }

        public async Task UpdateAsync(int id, UpdateCorrencyDto updateCorrencyDto)
        {
            var corrency = await _context.Correncys.FindAsync(id);

            if (corrency == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            corrency.FullName = updateCorrencyDto.FullName;
            corrency.ShortName = updateCorrencyDto.ShortName;
            corrency.Code = updateCorrencyDto.Code;

            await _context.SaveChangesAsync();
        }
    }
}
