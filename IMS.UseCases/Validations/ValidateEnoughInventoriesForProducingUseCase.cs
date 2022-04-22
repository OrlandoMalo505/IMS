using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases
{
    public class ValidateEnoughInventoriesForProducingUseCase : IValidateEnoughInventoriesForProducingUseCase
    {
        private readonly IProductRepository _productRepository;

        public ValidateEnoughInventoriesForProducingUseCase(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }
        public async Task<bool> ExecuteAsync(Product product, int quantity)
        {
            var prod = await _productRepository.GetProductById(product.ProductId);
            foreach (var pi in prod.ProductInventories)
            {
                if (pi.InventoryQuantity * quantity > pi.Inventory.Quantity) return false;

            }
            return true;
        }
    }
}
