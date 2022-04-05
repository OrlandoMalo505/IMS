using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using Microsoft.EntityFrameworkCore;

namespace IMS.Plugins.EFCore
{
    public class InventoryRepository : IInventoryRepository
    {
        private readonly IMSContext _context;

        public InventoryRepository(IMSContext context)
        {
            _context = context;
        }

        public async Task AddInventoryAsync(Inventory inventory)
        {
            if (_context.Inventories.Any(inv => inv.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }
            _context.Inventories.Add(inventory);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Inventory>> GetInventoriesByName(string name)
        {
            return await _context.Inventories.Where(x => x.InventoryName.Contains(name, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(name)).ToListAsync();
        }

        public async Task<Inventory?> GetInventoryById(int id)
        {
            var inv = await _context.Inventories.FirstOrDefaultAsync(inv => inv.InventoryId == id);
            if(inv == null)
            {
                return null;
            }
            return inv; 
        }

        public async Task UpdateInventory(Inventory inventory)
        {
            var inv = await _context.Inventories.FindAsync(inventory.InventoryId);

            if (_context.Inventories.Any(x => x.InventoryId != inventory.InventoryId && x.InventoryName.Equals(inventory.InventoryName, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }
          
            else if(inv != null)
            {
                inv.InventoryName = inventory.InventoryName;
                inv.Price = inventory.Price;
                inv.Quantity = inventory.Quantity;

                await _context.SaveChangesAsync();
            }
        }
    }
}