
using CashBox.Repository.Dtos.DistrictDtos;
using Microsoft.EntityFrameworkCore;
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
            };
        }

        public async Task<List<DistrictDto>> GetListAsync(DistrictFilterDto districtFilterDto)
        {
            var district = _context.Districts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(districtFilterDto.FullName))
                district = district.Where(x => x.FullName.ToLower().Contains(districtFilterDto.FullName.ToLower()));
            if (!string.IsNullOrWhiteSpace(districtFilterDto.Code))
                district = district.Where(x => x.Code.ToLower().Contains(districtFilterDto.Code.ToLower()));
            if (!string.IsNullOrWhiteSpace(districtFilterDto.Region))
                district = district.Where(x => x.Region.ToLower().Contains(districtFilterDto.Region.ToLower()));
            if (districtFilterDto.Id != 0 && districtFilterDto.Id != null)
                district = district.Where(x => x.Id == districtFilterDto.Id);

            return await district.Select(u => new DistrictDto
            {
                Id = u.Id,
                FullName = u.FullName,
                Code = u.Code,
                Region = u.Region
            }).ToListAsync();
        }

        public async Task UpdateAsync(int id, UpdateDistrictDto updateDistrictDto)
        {
            var district = await _context.Districts.FindAsync(id);

            if (district == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            if (updateDistrictDto.FullName != null)
                district.FullName = updateDistrictDto.FullName;

            if (updateDistrictDto.Code != null)
                district.Code = updateDistrictDto.Code;

            if (updateDistrictDto.Region != null)
                district.Region = updateDistrictDto.Region;

            await _context.SaveChangesAsync();
        }
    }
}
