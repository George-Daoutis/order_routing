using Microsoft.EntityFrameworkCore;
using order_routing.Server.Data;
using order_routing.Server.Models;

namespace order_routing.Server.Services
{
    public interface IStoreService
    {
        public Task<List<StoreGetDTO>> GetAllStores();
        public Task<StoreGetDTO> GetCurrentStore();
    }
    public class StoreService: IStoreService
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<StoreService> _logger;
        public StoreService(OrderDbContext db_context, ILogger<StoreService> logger)
        {
            _dbContext = db_context;
            _logger = logger;
        }

        public async Task<List<StoreGetDTO>> GetAllStores()
        {
            try
            {
                var stores = await _dbContext.Stores
                    .Select(s => new StoreGetDTO { Id = s.Id, StoreDescription = s.StoreDescription, Address = s.Address, PhoneNumber = s.PhoneNumber})
                    .ToListAsync();

                if (stores.Count > 0)
                {
                    return stores;
                }
            else
                {
                    _logger.LogWarning("Store list is empty.");
                    return null!;
                }
            }
            catch (Exception err)
            {
                _logger.LogError("{err}", err);
                throw;
            }
        }

        public async Task<StoreGetDTO> GetCurrentStore()
        {
            try
            {
                var currentStore = await _dbContext.Stores
                    .Where(s => s.Id == 1)
                    .Select(s => new StoreGetDTO { Id = s.Id, StoreDescription = s.StoreDescription, Address = s.Address, PhoneNumber = s.PhoneNumber })
                    .FirstOrDefaultAsync();

                if (currentStore != null)
                {
                    return currentStore;
                }
                else
                {
                    _logger.LogWarning("Store list is empty.");
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
