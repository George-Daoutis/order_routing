using Microsoft.EntityFrameworkCore;
using order_routing.Server.Data;
using order_routing.Server.Models;

namespace order_routing.Server.Services
{
    public interface IProductService
    {
        public Task<List<ProductGetDTO>> GetAllProducts();
    }

    public class ProductsService: IProductService
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<ProductsService> _logger;

        public ProductsService(OrderDbContext dbContext, ILogger<ProductsService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<List<ProductGetDTO>> GetAllProducts()
        {
            try
            {
                var products = await _dbContext.Products
                .Select(p => new ProductGetDTO { Id = p.Id, Description = p.Description, ProductCode = p.ProductCode })
                .ToListAsync();

                if (products.Count > 0)
                {
                    return products;
                }
                else
                {
                    _logger.LogWarning("Products list is empty.");
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
