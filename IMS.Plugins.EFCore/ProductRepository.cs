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
    public class ProductRepository : IProductRepository
    {
        private readonly IMSContext _context;

        public ProductRepository(IMSContext context)
        {
            _context = context;
        }

        public async Task AddProduct(Product product)
        {
            //if (_context.Products.Any(x => x.ProductName.Equals(product.ProductName, StringComparison.OrdinalIgnoreCase)))
            //{
            //    return;
            //}
            if (_context.Products.Any(x => x.ProductName.ToLower() == product.ProductName.ToLower()))
            {
                return;
            }
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteProduct(int id)
        {
            var prod = await _context.Products.FirstOrDefaultAsync(x => x.ProductId == id);
            if(prod == null)
            {
                return;
            }
            prod.IsActive = false;
            await _context.SaveChangesAsync();  
        }

        public async Task<Product> GetProductById(int id)
        {
            var prod = await _context.Products.Include(i => i.ProductInventories).ThenInclude(x => x.Inventory).FirstOrDefaultAsync(p => p.ProductId == id);
            if (prod == null)
            {
                return null;
            }
            return prod;
        }

        public async Task<List<Product>> GetProductsByName(string name)
        {
            return await _context.Products.Where(x => (x.ProductName.ToLower().IndexOf(name.ToLower()) >= 0 
                                                 || string.IsNullOrWhiteSpace(name)) && x.IsActive == true).ToListAsync();
        }

        public async Task UpdateProduct(Product product)
        {
            var prod = await _context.Products.FindAsync(product.ProductId);

            if (_context.Products.Any(x => x.ProductId != product.ProductId && x.ProductName.ToLower() == product.ProductName.ToLower()))
            {
                return;
            }

            else if (prod != null)
            {
                prod.ProductName = product.ProductName;
                prod.Price = product.Price;
                prod.Quantity = product.Quantity;
                prod.ProductInventories = product.ProductInventories;   

                await _context.SaveChangesAsync();
            }
        }
    }
}
