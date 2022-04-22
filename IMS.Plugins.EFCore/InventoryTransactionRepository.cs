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
    public class InventoryTransactionRepository : IInventoryTransactionRepository
    {
        private readonly IMSContext _context;

        public InventoryTransactionRepository(IMSContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<InventoryTransaction>> GetInventoryTransactionsAsync(string inventoryName, DateTime? dateFrom,
            DateTime? dateTo, InventoryTransactionType? transactionType)
        {
            var query = from it in _context.InventoryTransactions
                        join inv in _context.Inventories on it.InventoryId equals inv.InventoryId
                        where
                        (string.IsNullOrWhiteSpace(inventoryName) || inv.InventoryName.Contains(inventoryName, StringComparison.OrdinalIgnoreCase)) &&
                        (!dateFrom.HasValue || it.TransactionDate >= dateFrom) &&
                        (!dateTo.HasValue || it.TransactionDate <= dateTo) &&
                        (!transactionType.HasValue || it.ActivityType == transactionType)
                        select it;
            return await query.ToListAsync();  
            
        }

        public async Task PurchaseInventory(string poNumber, Inventory inventory, int quantity, double price, string doneBy)
        {
            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                InventoryId = inventory.InventoryId,
                PoNumber = poNumber,
                QuantityBefore = quantity,
                Inventory = inventory,
                ActivityType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                UnitPrice = price * quantity
            }); 
            await _context.SaveChangesAsync();
        }
    }
}
