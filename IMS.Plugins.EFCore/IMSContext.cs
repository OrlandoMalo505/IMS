using IMS.CoreBusiness;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Plugins.EFCore
{
    public class IMSContext : DbContext
    {

        public IMSContext(DbContextOptions<IMSContext> options) : base(options)
        {
        }

        public DbSet<Inventory> Inventories { get; set; }
        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Inventory>().HasData(
                new Inventory { InventoryId = 1, InventoryName = "Engine", Price = 1000, Quantity = 1},
                new Inventory { InventoryId = 2, InventoryName = "Body", Price = 4000, Quantity = 5},
                new Inventory { InventoryId = 3, InventoryName = "Wheel", Price = 200, Quantity = 100},
                new Inventory { InventoryId = 4, InventoryName = "Seat", Price = 70, Quantity = 10}
                );

            modelBuilder.Entity<Product>().HasData(
                new Product { ProductId = 1, ProductName = "Engine", Price = 1000, Quantity = 1 },
                new Product { ProductId = 2, ProductName = "Body", Price = 4000, Quantity = 5 },
                new Product { ProductId = 3, ProductName = "Wheel", Price = 200, Quantity = 100 },
                new Product { ProductId = 4, ProductName = "Seat", Price = 70, Quantity = 10 }
                );
        }
    }
}
