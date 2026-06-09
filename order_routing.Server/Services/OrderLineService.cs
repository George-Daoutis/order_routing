using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using order_routing.Server.Data;
using order_routing.Server.Models;

namespace order_routing.Server.Services
{
    public interface IOrderLineService
    {
        public Task<OrderLine> NewOrder(ProductGetDTO productDTO, StoreGetDTO storeDTO, decimal amount);
        public Task<OrderLine> OrderLineStatusChange(int orderId, OrderLineStatus lineStatus);
    }
    public class OrderLineService : IOrderLineService
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderLineService> _logger;

        public OrderLineService(OrderDbContext dbContext, ILogger<OrderLineService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<OrderLine> NewOrder(ProductGetDTO productDTO, StoreGetDTO storeDTO, decimal amount)
        {
            try
            {
                var product = await _dbContext.Products
                    .FirstOrDefaultAsync(p => p.ProductCode == productDTO.ProductCode);

                var store = await _dbContext.Stores
                    .FirstOrDefaultAsync(s => s.Id == storeDTO.Id);

                if (product != null && store != null)
                {
                    var orderLine = new OrderLine
                    {
                        ProductId = product.Id,
                        StoreId = store.Id,
                        Amount = amount,
                        OrderLineStatus = OrderLineStatus.Requested
                    };
                    await _dbContext.OrderLines.AddAsync(orderLine);
                    await _dbContext.SaveChangesAsync();
                    return orderLine;
                }
                else
                {
                    _logger.LogWarning("Product {product} or Store {store} not found", productDTO.ProductCode, storeDTO.Id);
                    return null!;
                }
            }
            catch (Exception err)
            {
                _logger.LogError("{err}", err);
                throw;
            }
        }

        public async Task<OrderLine> OrderLineStatusChange(int orderId, OrderLineStatus linestatus)
        {
            try
            {
                var order = await _dbContext.OrderLines
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order != null)
                {
                    order.OrderLineStatus = linestatus;
                    await _dbContext.SaveChangesAsync();
                    return order;
                }
                else
                {
                    _logger.LogWarning("Order {order} was not found,", orderId);
                    return null!;
                }
            }
            catch (Exception err)
            {
                _logger.LogError("{err}", err);
                throw;
            }
        }

        public async Task<OrderLine> AddOrderFulfillment(OrderlineFulfillmentCreateDTO fulfillmentDTO)
        {
            try
            {
                var order = await _dbContext.OrderLines
                    .FirstOrDefaultAsync(o => o.Id == fulfillmentDTO.OrderLineId);

                if (order != null)
                {
                    var sum = await _dbContext.OrderLineFullfillments
                        .Where(o => o.OrderLineId == fulfillmentDTO.OrderLineId)
                        .SumAsync(o => o.Quantity);
                    
                    sum += fulfillmentDTO.Quantity;

                    if (sum >= order.Amount)
                    {
                        order.OrderLineStatus = OrderLineStatus.Completed;
                    }
                    else
                    {
                        order.OrderLineStatus = OrderLineStatus.PartiallyFullfilled;
                    }

                    var orderFulfillement = new OrderLineFulfillment { StoreId = fulfillmentDTO.StoreId, OrderLineId = order.Id, Quantity = fulfillmentDTO.Quantity };

                    await _dbContext.OrderLineFullfillments.AddAsync(orderFulfillement);
                    await _dbContext.SaveChangesAsync();
                    return order;
                }

                else
                {
                    _logger.LogWarning("Order {order} was not found,", fulfillmentDTO.OrderLineId);
                    return null!;
                }
            }
            catch (Exception err)
            {
                _logger.LogError("{err}", err);
                throw;
            }
        }
    }
}
