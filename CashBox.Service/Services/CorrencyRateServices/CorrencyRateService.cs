using CashBox.Repository.Dtos.CorrencyRateDtos;
using Microsoft.EntityFrameworkCore;
using Repository.Data;
using Repository.Entity;

namespace CashBox.Service.Services.CorrencyRateServices
{
    public class CurrencyRateService : ICurrencyRateService
    {
        private readonly AppDbContext _context;
        public CurrencyRateService(AppDbContext context)
        {
            _context = context;
        }
        public async Task CreateAsync(CreateCurrencyRateDto createCorrencyRateDto)
        {
            var correncyRate = new CorrencyRate()
            {
                CurrencyId = createCorrencyRateDto.CurrencyId,
                Rate = createCorrencyRateDto.Rate,
                Date = DateTime.UtcNow
            };
            await _context.CorrencyRates.AddAsync(correncyRate);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var correncyRate = await _context.CorrencyRates.FindAsync(id);

            if (correncyRate == null)
                throw new KeyNotFoundException($"{id} o'chirilmadi");

            _context.CorrencyRates.Remove(correncyRate);
            await _context.SaveChangesAsync();
        }
        public async Task<CurrencyRateDto> GetAsync(int id)
        {
            var correncyRate = await _context.CorrencyRates.FindAsync(id);
            if (correncyRate == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new CurrencyRateDto
            {
                Id = correncyRate.Id,
                CurrencyId = correncyRate.CurrencyId,
                Rate = correncyRate.Rate,
                Date = correncyRate.Date
            };
        }

        public async Task<List<CurrencyRateDto>> GetListAsync(CurrencyRateFilterDto currencyRateFilterDto)
        {
            var currencyRate = _context.CorrencyRates.AsQueryable();

            if (currencyRateFilterDto != null)
                currencyRate = currencyRate.Where(x => x.Id == currencyRateFilterDto.Id);
            if (currencyRateFilterDto.CurrencyId != 0 && currencyRateFilterDto.CurrencyId != null)
                currencyRate = currencyRate.Where(x => x.CurrencyId == currencyRateFilterDto.CurrencyId);
            if (currencyRateFilterDto.Date != null)
                currencyRate = currencyRate.Where(x => x.Date == currencyRateFilterDto.Date);

            return await currencyRate.Select(u => new CurrencyRateDto
            {
                Id = u.Id,
                CurrencyId = u.CurrencyId,
                Rate = u.Rate,
                Date = u.Date,
            }).ToListAsync();
        }

        public async Task UpdateAsync(int id, UpdateCurrencyRateDto updateCurrencyRateDto)
        {
            var currencyRate = await _context.CorrencyRates.FindAsync(id);

            if (currencyRate == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            if (updateCurrencyRateDto.CurrencyId.HasValue)
                currencyRate.CurrencyId = updateCurrencyRateDto.CurrencyId.Value;

            if (updateCurrencyRateDto.Rate.HasValue)
                currencyRate.Rate = updateCurrencyRateDto.Rate.Value;

            if (updateCurrencyRateDto.Date.HasValue)
                currencyRate.Date = updateCurrencyRateDto.Date.Value;

            await _context.SaveChangesAsync();
        }
    }
}
