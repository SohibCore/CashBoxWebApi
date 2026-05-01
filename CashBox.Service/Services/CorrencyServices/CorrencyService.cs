using CashBox.Repository.Dtos.CorrencyDtos;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entity;

namespace CashBox.Service.Services.CorrencyServices
{
    public class CorrencyService : ICurrencyService
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
            var corrency = await _context.Correncys.FindAsync(id);

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
        public async Task<List<CorrencyDto>> GetListAsync(CurrencyFilterDto currencyFilterDto)
        {
            var currency = _context.Correncys.AsQueryable();

            if (!string.IsNullOrWhiteSpace(currencyFilterDto.FullName))
                currency = currency.Where(x => x.FullName.ToLower().Contains(currencyFilterDto.FullName.ToLower()));
            if (!string.IsNullOrWhiteSpace(currencyFilterDto.ShortName))
                currency = currency.Where(x => x.ShortName.ToLower().Contains(currencyFilterDto.ShortName));
            if (!string.IsNullOrWhiteSpace(currencyFilterDto.Code))
                currency = currency.Where(x => x.Code.ToLower().Contains(currencyFilterDto.Code.ToLower()));
            if (currencyFilterDto.Id != 0 && currencyFilterDto.Id != null)
                currency = currency.Where(x => x.Id == currencyFilterDto.Id);

            return await currency.Select(u => new CorrencyDto
            {
                Id = u.Id,
                FullName = u.FullName,
                ShortName = u.ShortName,
                Code = u.Code
            }).ToListAsync();
        }

        public async Task UpdateAsync(int id, UpdateCorrencyDto updateCorrencyDto)
        {
            var corrency = await _context.Correncys.FindAsync(id);

            if (corrency == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            if (updateCorrencyDto is not null)
            {
                corrency.FullName = updateCorrencyDto.FullName;
                corrency.ShortName = updateCorrencyDto.ShortName;
                corrency.Code = updateCorrencyDto.Code;
            }

            await _context.SaveChangesAsync();
        }
    }
}
