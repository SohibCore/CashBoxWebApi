using CashBox.Repository.Entity;

namespace CashBox.Service.Services.ExelServices
{
    public interface IExelService
    {
        byte[] ExportProducts(List<Product> data);
    }
}
