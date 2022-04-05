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
        public async Task<List<Product>> GetProductsByName(string name)
        {
            return await _context.Products.Where(x => x.ProductName.Contains(name, StringComparison.OrdinalIgnoreCase) || string.IsNullOrWhiteSpace(name)).ToListAsync();
        }
    }
}
