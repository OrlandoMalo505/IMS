using IMS.CoreBusiness;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases.PluginInterfaces
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsByName(string name);
        Task AddProduct(Product product);
        Task<Product> GetProductById(int id);
        Task UpdateProduct(Product product);
        Task DeleteProduct(int id);
    }
}
