using CashBox.Repository.Dtos.ProductDtos;

namespace CashBox.Service.Services.ProductServices
{
    public interface IProductService
    {
        Task<List<ProductDto>> GetListAsync(ProductFilterDto productFilterDto);
        Task<ProductDto> GetAsync(int id);
        Task DeleteAsync(int id);
        Task UpdateAsync(int id, UpdateProductDto updateProductDto);
        Task CreateAsync(CreateProductDto createProductDto);
    }
}
