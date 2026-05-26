using CashBox.Repository.Dtos.SupplierDtos;

namespace CashBox.Service.Services.SupplierServices
{
    public interface ISupplierService
    {
        Task<SupplierDto> GetAsync(int id);
        Task<List<SupplierDto>> GetListAsync(SupplierFilterDto supplierFilterDto);
        Task CreateAsync(CreateSupplierDto createSupplierDto);
        Task UpdateAsync(int id, UpdateSupplierDto updateSupplierDto);
        Task DeleteAsync(int id);
    }
}
