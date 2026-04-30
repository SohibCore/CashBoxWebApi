using CashBox.Repository.Dtos.CorrencyRateDtos;
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
                throw new KeyNotFoundException($"{correncyRate} topilmadi");

            return new CurrencyRateDto
            {
                Id = correncyRate.Id,
                CurrencyId = correncyRate.CurrencyId,
                Rate = correncyRate.Rate,
                Date = correncyRate.Date
            };
        }
        public async Task UpdateAsync(int id, UpdateCurrencyRateDto updateCurrencyRateDto)
        {
            var currencyRate = await _context.CorrencyRates.FindAsync(id);

            if (currencyRate == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            currencyRate.CurrencyId = (int)updateCurrencyRateDto.CurrencyId;
            currencyRate.Rate = (decimal)updateCurrencyRateDto.Rate;
            currencyRate.Date = (DateTime)updateCurrencyRateDto.Date;

            await _context.SaveChangesAsync();
        }
    }
}
