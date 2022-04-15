using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
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
        public async Task PurchaseInventory(string poNumber, Inventory inventory, int quantity, double price, string doneBy)
        {
            _context.InventoryTransactions.Add(new InventoryTransaction
            {
                InventoryId = inventory.InventoryId,
                PoNumber = poNumber,
                QuantityBefore = quantity,
                Inventory = inventory,
                InventoryType = InventoryTransactionType.PurchaseInventory,
                QuantityAfter = inventory.Quantity + quantity,
                TransactionDate = DateTime.Now,
                DoneBy = doneBy,
                Cost = price * quantity
            }); 
            await _context.SaveChangesAsync();
        }
    }
}
