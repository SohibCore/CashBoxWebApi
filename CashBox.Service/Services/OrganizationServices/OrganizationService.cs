using CashBox.Repository.Dtos.OrganizationDtos;
using Microsoft.EntityFrameworkCore;
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

        public async Task<List<OrganizationDto>> GetListAsync(OrganizationFilterDto organizationFilterDto)
        {
            var organization = _context.Organizations.AsQueryable();

            if (!string.IsNullOrWhiteSpace(organizationFilterDto.FullName))
                organization = organization.Where(x => x.FullName.ToLower().Contains(organizationFilterDto.FullName.ToLower()));
            if (!string.IsNullOrWhiteSpace(organizationFilterDto.Inn))
                organization = organization.Where(x => x.Inn.ToLower().Contains(organizationFilterDto.Inn.ToLower()));
            if (!string.IsNullOrWhiteSpace(organizationFilterDto.ShortName))
                organization = organization.Where(x => x.ShortName.ToLower().Contains(organizationFilterDto.ShortName.ToLower()));
            if (!string.IsNullOrWhiteSpace(organizationFilterDto.District))
                organization = organization.Where(x => x.District.ToLower().Contains(organizationFilterDto.District.ToLower()));
            if (organizationFilterDto.Id != 0 && organizationFilterDto.Id != null)
                organization = organization.Where(x => x.Id == organizationFilterDto.Id);
            if (organizationFilterDto.RegionId != 0 && organizationFilterDto.RegionId != null)
                organization = organization.Where(x => x.RegionId == organizationFilterDto.RegionId);

            return await organization.Select(u => new OrganizationDto
            {
                Id = u.Id,
                FullName = u.FullName,
                ShortName = u.ShortName,
                Inn = u.Inn,
                RegionId = u.RegionId,
                District = u.District
            }).ToListAsync();
        }
    }
}
