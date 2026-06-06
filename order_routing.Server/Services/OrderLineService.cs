using order_routing.Server.Data;

namespace order_routing.Server.Services
{
    public interface IOrderLineService
    {
        public Task<string> TempTask(string temp);
    }
    public class OrderLineService: IOrderLineService
    {
        private readonly OrderDbContext _dbContext;
        private readonly ILogger<OrderLineService> _logger;

        public OrderLineService(OrderDbContext dbContext, ILogger<OrderLineService> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        public async Task<string> TempTask(string temp)
        {
            try
            {
                _logger.LogWarning("");
                return "a";
            }
            catch (Exception err)
            {
                _logger.LogError(err, "");
                throw;
            }
        }
    }
}
