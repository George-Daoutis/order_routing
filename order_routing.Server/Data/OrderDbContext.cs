using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.DataProtection.EntityFrameworkCore;
using order_routing.Server.Models;

namespace order_routing.Server.Data
{
    public class OrderDbContext: DbContext, IDataProtectionKeyContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
        {
            
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>().HasData(
                new Product { Id = 1, Description = "Item 1", ProductCode = "ITEM001" },
                new Product { Id = 2, Description = "Item 2", ProductCode = "ITEM002" },
                new Product { Id = 3, Description = "Item 3", ProductCode = "ITEM003" }
                );

            modelBuilder.Entity<Store>().HasData(
                new Store { Id = 1, Address = "Address 1", StoreDescription = "Store 1", PhoneNumber = "2101234567"},
                new Store { Id = 2, Address = "Address 2", StoreDescription = "Store 2", PhoneNumber = "2104501929" },
                new Store { Id = 3, Address = "Address 3", StoreDescription = "Store 3", PhoneNumber = "2106316478" }
                );


            modelBuilder.Entity<OrderLine>()
                .HasOne(s => s.Store)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(s => s.StoreId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderLine>()
                .HasOne(p => p.Product)
                .WithMany(o => o.OrderLines)
                .HasForeignKey(p => p.ProductId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderLineFulfillment>()
                .HasOne(o => o.OrderLine)
                .WithMany(o => o.OrderLineFulfillment)
                .HasForeignKey(p => p.OrderLineId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<OrderLineFulfillment>()
                .HasOne(s => s.Store)
                .WithMany(o => o.OrderLineFulfillments)
                .HasForeignKey(s => s.StoreId)
                .OnDelete(DeleteBehavior.Restrict);
        }

        protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
        {
            configurationBuilder.Properties<decimal>().HavePrecision(18, 3);
        }

        public DbSet<OrderLine> OrderLines { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Store> Stores { get; set; }
        public DbSet<OrderLineFulfillment> OrderLineFullfillments { get; set; }

        public DbSet<DataProtectionKey> DataProtectionKeys { get; set; } = null!;
    }
}
