using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using order_routing.Server.Data;
using order_routing.Server.Models;

namespace order_routing.Server.Services
{
    public interface IOrderLineService
    {
        public Task<OrderLineGetDTO> NewOrder(OrderLineCreateDTO orderlineCreateDTO);
        public Task<List<OrderLineGetDTO>> GetAllOrderLines();
        public Task<OrderLineGetDTO> OrderLineStatusChange(int orderId, OrderLineStatus lineStatus);
        public Task<OrderLineGetDTO> AddOrderFulfillment(OrderlineFulfillmentCreateDTO fulfillmentDTO);
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

        public async Task<OrderLineGetDTO> NewOrder(OrderLineCreateDTO orderlineCreateDTO)
        {
            try
            {
                var product = await _dbContext.Products
                    .FirstOrDefaultAsync(p => p.Id == orderlineCreateDTO.ProductId);

                var store = await _dbContext.Stores
                    .FirstOrDefaultAsync(s => s.Id == orderlineCreateDTO.StoreId);

                if (product != null && store != null)
                {
                    var orderLine = new OrderLine
                    {
                        ProductId = product.Id,
                        StoreId = store.Id,
                        Amount = orderlineCreateDTO.Amount,
                        OrderLineStatus = OrderLineStatus.Requested
                    };
                    await _dbContext.OrderLines.AddAsync(orderLine);
                    await _dbContext.SaveChangesAsync();
                    return new OrderLineGetDTO
                    {
                        Id = orderLine.Id,
                        CreationDate = orderLine.CreationDate,
                        StoreId = orderLine.StoreId,
                        ProductId = orderLine.ProductId,
                        Amount = orderLine.Amount,
                        OrderLineFulfillment = orderLine.OrderLineFulfillment.Select(f => new OrderlineFulfillmentGetDTO
                        {
                            Id = f.Id,
                            StoreId = f.StoreId,
                            Quantity = f.Quantity,
                            CreationDate = f.CreationDate,
                            OrderLineId = f.OrderLineId
                        }).ToList(),
                        OrderLineStatus = orderLine.OrderLineStatus
                    };
                }
                else
                {
                    _logger.LogWarning("Product {product} or Store {store} not found", orderlineCreateDTO.ProductId, orderlineCreateDTO.StoreId);
                    return null!;
                }
            }
            catch (Exception err)
            {
                _logger.LogError("{err}", err);
                throw;
            }
        }

        public async Task<List<OrderLineGetDTO>> GetAllOrderLines()
        {
            try
            {
                var orders = await _dbContext.OrderLines
                .Select(o => new OrderLineGetDTO
                { Id = o.Id,
                    Amount = o.Amount,
                    CreationDate = o.CreationDate,
                    OrderLineStatus = o.OrderLineStatus,
                    ProductId = o.ProductId,
                    StoreId = o.StoreId,
                    OrderLineFulfillment = o.OrderLineFulfillment.Select(f => new OrderlineFulfillmentGetDTO
                    {
                        Id = f.Id,
                        StoreId = f.StoreId,
                        Quantity = f.Quantity,
                        CreationDate = f.CreationDate,
                        OrderLineId = f.OrderLineId
                    }).ToList()
                }).ToListAsync();

                if (orders.Count > 0)
                {
                    return orders;
                }
                else
                {
                    _logger.LogWarning("Orders not found.");
                    return [];
                }
            }
            catch (Exception err)
            {
                _logger.LogError("{err}", err);
                throw;
            }
        }

        public async Task<OrderLineGetDTO> OrderLineStatusChange(int orderId, OrderLineStatus linestatus)
        {
            try
            {
                var order = await _dbContext.OrderLines
                    .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order != null)
                {
                    order.OrderLineStatus = linestatus;
                    await _dbContext.SaveChangesAsync();
                    return new OrderLineGetDTO
                    {
                        Id = order.Id,
                        CreationDate = order.CreationDate,
                        StoreId = order.StoreId,
                        ProductId = order.ProductId,
                        Amount = order.Amount,
                        OrderLineFulfillment = order.OrderLineFulfillment.Select(f => new OrderlineFulfillmentGetDTO
                        {
                            Id = f.Id,
                            StoreId = f.StoreId,
                            Quantity = f.Quantity,
                            CreationDate = f.CreationDate,
                            OrderLineId = f.OrderLineId
                        }).ToList(),
                        OrderLineStatus = order.OrderLineStatus
                    };
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

        public async Task<OrderLineGetDTO> AddOrderFulfillment(OrderlineFulfillmentCreateDTO fulfillmentDTO)
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
                    return new OrderLineGetDTO
                    {
                        Id = order.Id,
                        CreationDate = order.CreationDate,
                        StoreId = order.StoreId,
                        ProductId = order.ProductId,
                        Amount = order.Amount,
                        OrderLineFulfillment = order.OrderLineFulfillment.Select(f => new OrderlineFulfillmentGetDTO
                        {
                            Id = f.Id,
                            StoreId = f.StoreId,
                            Quantity = f.Quantity,
                            CreationDate = f.CreationDate,
                            OrderLineId = f.OrderLineId
                        }).ToList(),
                        OrderLineStatus = order.OrderLineStatus
                    };
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
