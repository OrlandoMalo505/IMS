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

        public ProductTransactionRepository(IMSContext context, IProductRepository productRepository)
        {
            _context = context;
        }

        public async Task ProduceAsync(string productionNumber, Product product, int quantity, double price, string doneBy)
        {
            var prod = await _context.Products
                .Include(x => x.ProductInventories)
                .ThenInclude(x => x.Inventory)
                .FirstOrDefaultAsync(x => x.ProductId == product.ProductId);

            if (prod != null)
            {
                foreach (var p in prod.ProductInventories)
                {
                    int qtyBefore = p.Inventory.Quantity;
                    p.Inventory.Quantity -= quantity * p.InventoryQuantity;

                    _context.InventoryTransactions.Add(new InventoryTransaction
                    {
                        ProductionNumber = productionNumber,    
                        InventoryId = p.Inventory.InventoryId,
                        QuantityBefore = qtyBefore,
                        Inventory = p.Inventory,
                        ActivityType = InventoryTransactionType.ProduceProduct,
                        QuantityAfter = p.Inventory.Quantity,
                        TransactionDate = DateTime.Now,
                        DoneBy = doneBy,
                        UnitPrice = price * quantity
                    });
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

        public async Task SellProductAsync(string salesOrderNumber, Product product, int quantity, double price, string doneBy)
        {
            _context.ProductTransactions.Add(new ProductTransaction
            {
                SalesOrderNumber = salesOrderNumber,
                ProductId = product.ProductId,
                QuantityBefore = product.Quantity,
                QuantityAfter= product.Quantity - quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                ActivityType = ProductTransactionType.SellProduct,
                UnitPrice= price
            });
            await _context.SaveChangesAsync();
        }


        public async Task<IEnumerable<ProductTransaction>> GetProductTransactionsAsync(string productName, DateTime? dateFrom, DateTime? dateTo, ProductTransactionType? transactionType)
        {
            if(dateTo.HasValue)
                dateTo = dateTo.Value.AddDays(1);

            var query = from pt in _context.ProductTransactions
                        join prod in _context.Products on pt.ProductId equals prod.ProductId
                        where
                        (string.IsNullOrWhiteSpace(productName) || prod.ProductName.ToLower().IndexOf(productName.ToLower()) >= 0)
                        && (!dateFrom.HasValue || pt.TransactionDate >= dateFrom.Value.Date)
                        && (!dateTo.HasValue || pt.TransactionDate <= dateTo.Value.Date)
                        && (!transactionType.HasValue || pt.ActivityType == transactionType)
                        select pt;
            return await query.Include(x => x.Product).ToListAsync();
        }
    }
}
