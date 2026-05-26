using CashBox.Repository.Dtos.SupplierDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Services.AccountServices;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.SupplierServices
{
    public class SupplierService : ISupplierService
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;
        public SupplierService(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }

        public async Task CreateAsync(CreateSupplierDto createSupplierDto)
        {
            var supplier = new Supplier
            {
                Inn = createSupplierDto.Inn,
                Code = createSupplierDto.Code,
                CreatedAt = DateTime.UtcNow,
                CreatedUserId = _account.UserId
            };
            await _context.AddAsync(supplier);
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

            if(supplier == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            supplier.Inn = updateSupplierDto.Inn ?? supplier.Inn;
            supplier.Code = updateSupplierDto.Code ?? supplier.Code;
            supplier.ModifiedAt = DateTime.UtcNow;
            supplier.ModifiedUserId = _account.UserId;

            _context.Suppliers.Update(supplier);
            await _context.SaveChangesAsync();
        }
    }
}
