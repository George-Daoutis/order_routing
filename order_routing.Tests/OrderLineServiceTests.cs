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

            var orderlineCreate = new OrderLineCreateDTO { ProductId = 1, StoreId = 1, Amount = 10 };

            var result = await service.NewOrder(orderlineCreate);

            Assert.NotNull(result);
            Assert.Equal(10, result.Amount);
            Assert.Equal(1, result.ProductId);
            Assert.Equal(1, result.StoreId);
            Assert.Equal(OrderLineStatus.Requested, result.OrderLineStatus);

            var orderline = await dbContext.OrderLines.FirstOrDefaultAsync(p => p.ProductId == 1);
            Assert.NotNull(orderline);
            Assert.Equal(10, orderline.Amount);
            Assert.Equal(OrderLineStatus.Requested, orderline.OrderLineStatus);
        }

        [Fact]
        public async Task AddOrderFulfillment_WhenTotalQuantityMeetsAmount_ShouldSetStatusToCompleted()
        {
            var dbContext = GetInMemoryDbContext();

            var logger = NullLogger<OrderLineService>.Instance;

            var service = new OrderLineService(dbContext, logger);

            var existingOrder = new OrderLine
            {
                Id = 1,
                Amount = 10,
                OrderLineStatus = OrderLineStatus.Requested
            };
            await dbContext.OrderLines.AddAsync(existingOrder);
            await dbContext.SaveChangesAsync();

            var fulfillmentDto = new OrderlineFulfillmentCreateDTO
            {
                OrderLineId = 1,
                StoreId = 5,
                Quantity = 10
            };

            var result = await service.AddOrderFulfillment(fulfillmentDto);

            Assert.NotNull(result);
            Assert.Equal(OrderLineStatus.Completed, result.OrderLineStatus);

            var fulfillmentInDb = await dbContext.OrderLineFullfillments.FirstOrDefaultAsync(f => f.OrderLineId == 1);
            Assert.NotNull(fulfillmentInDb);
            Assert.Equal(10, fulfillmentInDb.Quantity);
        }
    }
}
