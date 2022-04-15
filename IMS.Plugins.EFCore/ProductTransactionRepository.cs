using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCore
{
    public class ProductTransactionRepository : IProductTransactionRepository
    {
        private readonly IMSContext _context;
        private readonly IProductRepository _productRepository;

        public ProductTransactionRepository(IMSContext context, IProductRepository productRepository)
        {
            _context = context;
            _productRepository = productRepository;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, double price, string doneBy)
        {
            //var prod = _productRepository.GetProductById(product.ProductId);
            var prod = await _context.Products
                .Include(x => x.ProductInventories)
                .ThenInclude(x => x.Inventory)
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (prod != null)
            {
                foreach (var p in prod.ProductInventories)
                {
                    p.Inventory.Quantity -= quantity * p.InventoryQuantity;
                }
            }
            _context.ProductTransactions.Add(new ProductTransaction
            {
                ProductionNumber = productionNumber,
                Product = product,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                ActivityType = ProductTransactionType.ProduceProduct,
                QuantityAfter = product.Quantity + quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price
            });

            await _context.SaveChangesAsync();
        }
    }
}
