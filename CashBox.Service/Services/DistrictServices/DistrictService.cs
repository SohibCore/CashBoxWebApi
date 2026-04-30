using CashBox.Repository.Dtos.DistrictDtos;
using Repository.Data;
using RepositoryLayer.Entity;

namespace CashBox.Service.Services.DistrictServices
{
    public class DistrictService : IDistrictService
    {
        private readonly AppDbContext _context;
        public DistrictService(AppDbContext contex)
        {
            _context = contex;
        }
        public async Task CreateAsync(CreateDistrictDto createDistrictDto)
        {
            var district = new District
            {
                FullName = createDistrictDto.FullName,
                Code = createDistrictDto.Code,
                Region = createDistrictDto.Region
            };
            await _context.Districts.AddAsync(district);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var district = await _context.Districts.FindAsync(id);

            if (district == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            _context.Districts.Remove(district);
            await _context.SaveChangesAsync();
        }
        public async Task<DistrictDto> GetAsync(int id)
        {
            var district = await _context.Districts.FindAsync(id);

            if (district == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new DistrictDto
            {
                Id = district.Id,
                FullName = district.FullName,
                Code = district.Code,
                Region = district.Region,
                CreateUserId = district.CreateUserId
            };
        }
        public async Task UpdateAsync(int id, UpdateDistrictDto updateDistrictDto)
        {
            var district = await _context.Districts.FindAsync(id);

            if (district == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            district.FullName = updateDistrictDto.FullName;
            district.Code = updateDistrictDto.Code;
            district.Region = updateDistrictDto.Region;

            await _context.SaveChangesAsync();
        }
    }
}
