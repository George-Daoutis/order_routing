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
