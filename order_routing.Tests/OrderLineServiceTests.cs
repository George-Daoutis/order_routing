using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using order_routing.Server.Data;
using order_routing.Server.Models;
using order_routing.Server.Services;

namespace order_routing.Tests
{
    public class OrderLineServiceTests
    {
        private OrderDbContext GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<OrderDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new OrderDbContext(options);
        }


        [Fact]
        public async Task NewOrder_ProductAndStoreExist_ShouldCreateNewOrder()
        {
            var dbContext = GetInMemoryDbContext();

            var logger = NullLogger<OrderLineService>.Instance;

            var service = new OrderLineService(dbContext, logger);

            var existingProduct = new Product { Id = 1, Description = "item 1", ProductCode = "12345" };
            var existingStore = new Store { Id = 1, Address = "address 1", StoreDescription = "store in adres 1", PhoneNumber = "1234567678" };
            await dbContext.Products.AddAsync(existingProduct);
            await dbContext.Stores.AddAsync(existingStore);
            await dbContext.SaveChangesAsync();

            var product = new ProductGetDTO { Description = "item 1", ProductCode = "12345" };
            var store = new StoreGetDTO { Id = 1, Address = "address 1", StoreDescription = "store in adres 1", PhoneNumber = "1234567678" };

            var result = await service.NewOrder(product, store, 10);

            Assert.NotNull(result);
            Assert.Equal(10, result.Amount);
            Assert.Equal(existingProduct.Id, result.ProductId);
            Assert.Equal(existingStore.Id, result.StoreId);
            Assert.Equal(OrderLineStatus.Requested, result.OrderLineStatus);

            var orderline = await dbContext.OrderLines.FirstOrDefaultAsync(p => p.ProductId == existingProduct.Id);
            Assert.NotNull(orderline);
            Assert.Equal(10, orderline.Amount);
            Assert.Equal(OrderLineStatus.Requested, orderline.OrderLineStatus);
        }
    }
}
