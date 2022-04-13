using IMS.CoreBusiness.Validations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.CoreBusiness
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        public string ProductName { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage ="Quantity must be greater or equal to 1")]
        [Required]
        public int Quantity { get; set; }

        [Range(1, double.MaxValue, ErrorMessage ="Price must be greater or equal to 1")]
        [Product_EnsurePriceIsGreaterThanInventoriesPrice]
        [Required]
        public double Price { get; set; }

        public List<ProductInventory> ProductInventories { get; set; }

        public double TotalInventoryCost()
        {
            return ProductInventories.Sum(x => x.Inventory?.Price * x.InventoryQuantity ?? 0);
        }
        public bool ValidatePricing()
        {
            if(ProductInventories == null || ProductInventories.Count <= 0)
            {
                return true;
            }

            if(TotalInventoryCost() > Price)
            {
                return false;
            }
            return true;
        }
    }
}
