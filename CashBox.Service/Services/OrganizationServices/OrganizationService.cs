using CashBox.Repository.Dtos.OrganizationDtos;
using Repository.Data;
using RepositoryLayer.Entity;

namespace CashBox.Service.Services.OrganizationServices
{
    public class OrganizationService : IOrganizationService
    {
        private readonly AppDbContext _context;
        public OrganizationService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<OrganizationDto> GetAsync(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new OrganizationDto
            {
                Id = organization.Id,
                Inn = organization.Inn,
                FullName = organization.FullName,
                ShortName = organization.ShortName,
                RegionId = organization.RegionId,
                District = organization.District
            };

        }
        public async Task CreateAsync(CreateOrganizationDto createOrganizationDto)
        {
            var organization = new Organization
            {
                Inn = createOrganizationDto.Inn,
                FullName = createOrganizationDto.FullName,
                ShortName = createOrganizationDto.ShortName,
                RegionId = createOrganizationDto.RegionId,
                District = createOrganizationDto.District,
            };
            await _context.Organizations.AddAsync(organization);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateAsync(int id, UpdateOrganizationDto updateOrganizationDto)
        {
            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
                throw new KeyNotFoundException($"{id} li foydalanuvchi topilmadi");

            organization.Inn = updateOrganizationDto.Inn;
            organization.FullName = updateOrganizationDto.FullName;
            organization.ShortName = updateOrganizationDto.ShortName;
            organization.RegionId = updateOrganizationDto.RegionId;
            organization.District = updateOrganizationDto.District;

            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var organization = await _context.Organizations.FindAsync(id);

            if (organization == null)
                throw new KeyNotFoundException($"{id} li foydalanuvchi topilmadi");

            _context.Organizations.Remove(organization);
            await _context.SaveChangesAsync();
        }
    }
}
