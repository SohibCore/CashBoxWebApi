using CashBox.Repository.Dtos.ProductDtos;
using CashBox.Repository.Entity;
using CashBox.Service.Services.AccountServices;
using Microsoft.EntityFrameworkCore;
using Repository.Data;

namespace CashBox.Service.Services.ProductServices
{
    public class ProductService : IProductService
    {
        private readonly AppDbContext _context;
        private readonly AccountService _account;

        public ProductService(AppDbContext context, AccountService account)
        {
            _context = context;
            _account = account;
        }

        public async Task CreateAsync(CreateProductDto createProductDto)
        {
            var product = new Product //Entity
            {
                Code = createProductDto.Code,
                Name = createProductDto.Name,
                OrganizationId = createProductDto.OrganizationId,
                DeliveredAt = createProductDto.DeliveredAt,
                CreatedUserId = _account.UserId,
                CreatedAt = DateTime.UtcNow
            };

            await _context.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
        }

        public async Task<ProductDto> GetAsync(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            return new ProductDto
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                OrganizationId = product.OrganizationId,
                DeliveredAt = product.DeliveredAt
            };
        }

        public async Task<List<ProductDto>> GetListAsync(ProductFilterDto productFilterDto)
        {
            var product = _context.Products.AsQueryable();

            if (product != null)
            {
                if (productFilterDto.Id.HasValue)
                    product = product.Where(x => x.Id == productFilterDto.Id);

                if (!string.IsNullOrWhiteSpace(productFilterDto.Code))
                    product = product.Where(x => x.Code.ToLower().Contains(productFilterDto.Code.ToLower()));

                if (!string.IsNullOrWhiteSpace(productFilterDto.Name))
                    product = product.Where(x => x.Name.ToLower().Contains(productFilterDto.Name.ToLower()));

                if (productFilterDto.OrganizationId.HasValue)
                    product = product.Where(x => x.OrganizationId == productFilterDto.OrganizationId);

                if (productFilterDto.DeliveredAt.HasValue)
                    product = product.Where(x => x.DeliveredAt == productFilterDto.DeliveredAt);
            }
            else
                throw new KeyNotFoundException($"{product} topilmadi");

            return await product.Select(u => new ProductDto
            {
                Id = u.Id,
                Name = u.Name,
                Code = u.Code,
                OrganizationId = u.OrganizationId,
                DeliveredAt = u.DeliveredAt,
            }).ToListAsync();

        }

        public async Task UpdateAsync(int id, UpdateProductDto updateProductDto)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                throw new KeyNotFoundException($"{id} topilmadi");

            if (!string.IsNullOrWhiteSpace(updateProductDto.Code))
                product.Code = updateProductDto.Code;

            if (!string.IsNullOrWhiteSpace(updateProductDto.Name))
                product.Name = updateProductDto.Name;

            if (updateProductDto.OrganizationId != 0 && updateProductDto.OrganizationId != null)
                product.OrganizationId = updateProductDto.OrganizationId;

            if (updateProductDto.DeliveredAt != null)
                product.DeliveredAt = updateProductDto.DeliveredAt;

            product.ModifiedAt = DateTime.UtcNow;
            product.ModifiedUserId = _account.UserId;

            await _context.SaveChangesAsync();
        }
    }
}
