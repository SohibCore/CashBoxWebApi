using CashBox.Repository.Dtos.SupplierDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Integrations.UzasboServices;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.SupplierServices
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;
        private readonly IUzasboService _uzasboService;
        public SupplierService(AppDbContext context, AccountService account, IUzasboService uzasboService)
        {
            _context = context;
            _account = account;
            _uzasboService = uzasboService;
        }

        public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto)
        {
            var org = await _context.Organizations.FirstOrDefaultAsync(o => o.Inn == dto.Inn);
            var sup = await _context.Suppliers.FirstOrDefaultAsync(s => s.Inn == dto.Inn);

            if (org == null)
                throw new Exception($"{org} bo'yicha tashkilot topilmadi");

            var newSupplier = new Supplier
            {
                Code = dto.Code,
                Inn = org.Inn,
            };
            await _context.Suppliers.AddAsync(newSupplier);
            await _context.SaveChangesAsync();

            return new SupplierDto
            {
                Id = newSupplier.Id,
                Inn = newSupplier.Inn,
                Code = newSupplier.Code
            };
        }

        public async Task<SupplierDto> GetAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new SupplierDto
            {
                Id = supplier.Id,
                Inn = supplier.Inn,
                Code = supplier.Code,
                ShortName = supplier.ShortName,
                Name = supplier.Name,
                Address = supplier.Address,
                Director = supplier.Director,
                Mfo = supplier.Mfo,
                Account = supplier.Account,
            };
        }

        public async Task<List<SupplierDto>> GetListAsync(SupplierFilterDto supplierFilterDto)
        {
            var suppliers = _context.Suppliers.AsQueryable();

            if (supplierFilterDto.Id.HasValue)
                suppliers = suppliers.Where(x => x.Id == supplierFilterDto.Id);
            if (!string.IsNullOrEmpty(supplierFilterDto.Inn))
                suppliers = suppliers.Where(x => x.Inn.Contains(supplierFilterDto.Inn));
            if (!string.IsNullOrEmpty(supplierFilterDto.Code))
                suppliers = suppliers.Where(x => x.Code.Contains(supplierFilterDto.Code));

            return await suppliers.Select(x => new SupplierDto
            {
                Id = x.Id,
                Inn = x.Inn,
                Code = x.Code,
            }).ToListAsync();
        }

        public async Task UpdateAsync(int id, UpdateSupplierDto updateSupplierDto)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            supplier.Inn = updateSupplierDto.Inn ?? supplier.Inn;
            supplier.Code = updateSupplierDto.Code ?? supplier.Code;
            supplier.ModifiedAt = DateTime.UtcNow;
            supplier.ModifiedUserId = _account.UserId;

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteAsync(int id)
        {
            var supplier = await _context.Suppliers.FindAsync(id);

            if (supplier == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            _context.Remove(supplier);
            await _context.SaveChangesAsync();
        }

        public async Task<SupplierDto> CreateByInnAsync(string inn)
        {
            var result = await _context.Suppliers.FirstOrDefaultAsync(x => x.Inn == inn);

            if (result is not null)
            {
                var supplier = await GetAsync(result.Id);
                return supplier;
            }

            var newSupplier = await _uzasboService.GetFacturaAsync(inn);

            if (newSupplier is null)
                throw new Exception($"INN {inn} bo'yicha ma'lumot topilmadi.");

            var newsupplier = new Supplier
            {
                Inn = newSupplier.Tin,
                Code = Convert.ToString(newSupplier.Na1Code),
                ShortName = newSupplier.ShortName,
                Name = newSupplier.Name,
                Address = newSupplier.Address,
                Mfo = newSupplier.Mfo,
                Account = newSupplier.Account,
                Director = newSupplier.Director,

                CreatedAt = DateTime.UtcNow,
                CreatedUserId = _account.UserId
            };
            await _context.Suppliers.AddAsync(newsupplier);
            await _context.SaveChangesAsync();

            return await GetAsync(newsupplier.Id);
        }

    }
}
