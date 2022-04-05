using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases
{
    public class ViewProductsByNameUseCase : IViewProductsByNameUseCase
    {
        private readonly IProductRepository _productRepository;

        public ViewProductsByNameUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<List<Product>> ExecuteAsync(string name = "")
        {
            return await _productRepository.GetProductsByName(name);
        }
    }
}
