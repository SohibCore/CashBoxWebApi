using CashBox.Repository.Dtos.RegionDtos;
using Repository.Data;
using RepositoryLayer.Entity;

namespace CashBox.Service.Services.RegionServices
{
    public class RegionService : IRegionService
    {
        private readonly AppDbContext _context;
        public RegionService(AppDbContext context) // Dependency Injection orqali AppDbContext ni qabul qilamiz
        {
            _context = context;
        }
        public async Task<RegionDto> GetAsync(int id)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new RegionDto
            {
                Id = region.Id,
                FullName = region.FullName,
                ShortName = region.ShortName,
                Code = region.Code
            };
        }
        public async Task CreateAsync(CreateRegionDto createRegionDto)
        {
            var region = new Region
            {
                FullName = createRegionDto.FullName,
                ShortName = createRegionDto.ShortName,
                Code = createRegionDto.Code,
                CreatedAt = DateTime.UtcNow,
                CreateUserId = 1,
                ModifiedUserId = 1,
                ModifiedAt = DateTime.UtcNow
            };
            await _context.Regions.AddAsync(region);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, UpdateRegionDto updateRegionDto)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null)
                throw new KeyNotFoundException($"{region} topilmadi");

            region.FullName = updateRegionDto.FullName;
            region.ShortName = updateRegionDto.ShortName;
            region.Code = updateRegionDto.Code;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var region = await _context.Regions.FindAsync(id);

            if (region == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            _context.Regions.Remove(region);
            await _context.SaveChangesAsync();
        }
    }
}
