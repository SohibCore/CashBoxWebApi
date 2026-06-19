using CashBox.Repository.Entity;
using RepositoryLayer.Entity;

namespace CashBox.Service.Services.ExelServices
{
    public interface IExelService
    {
        byte[] ExportProducts(List<Product> data);
        byte[] ExportUsers(List<User> data);
    }
}
