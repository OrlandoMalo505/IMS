using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases
{
    public class ProduceProductUseCase : IProduceProductUseCase
    {
        private readonly IInventoryRepository _inventoryRepository;
        private readonly IProductRepository _productRepository;
        private readonly IInventoryTransactionRepository _inventoryTransactionRepository;
        private readonly IProductTransactionRepository _productTransactionRepository;

        public ProduceProductUseCase(IInventoryRepository inventoryRepository, IProductRepository productRepository,
            IInventoryTransactionRepository inventoryTransactionRepository, IProductTransactionRepository productTransactionRepository)
        {
            _inventoryRepository = inventoryRepository;
            _productRepository = productRepository;
            _inventoryTransactionRepository = inventoryTransactionRepository;
            _productTransactionRepository = productTransactionRepository;
        }

        public async Task ExecuteAsync(string productionNumber, Product product, int quantity, double price, string doneBy)
        {
            await _productTransactionRepository.ProduceAsync(productionNumber, product, quantity, price, doneBy);

            product.Quantity += quantity;
            await _productRepository.UpdateProduct(product);
        }
    }
}
