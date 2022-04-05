using IMS.CoreBusiness;
using IMS.UseCases.PluginInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.UseCases
{
    public class EditInventoryUseCase : IEditInventoryUseCase
    {
        private readonly IInventoryRepository _inventory;

        public EditInventoryUseCase(IInventoryRepository inventory)
        {
            _inventory = inventory;
        }

        public async Task ExecuteAsync(Inventory inventory)
        {
            await _inventory.UpdateInventory(inventory);
        }
    }
}
