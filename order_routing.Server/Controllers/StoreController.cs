using Microsoft.AspNetCore.Mvc;
using order_routing.Server.Models;
using order_routing.Server.Services;

namespace order_routing.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StoreController: ControllerBase
    {
        private readonly IStoreService _storeService;
        private readonly ILogger<StoreController> _logger;
        public StoreController (IStoreService storeService, ILogger<StoreController> logger)
        {
            _storeService = storeService;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<StoreGetDTO>>> GetAllStores()
        {
            var response = await _storeService.GetAllStores();
            if (response != null)
            {
                return Ok(response);
            }
            else return BadRequest();
        }

        [HttpGet("current")]
        public async Task<ActionResult<StoreGetDTO>> GetCurrentStore()
        {
            var response = await _storeService.GetCurrentStore();
            if (response != null)
            {
                return Ok(response);
            }
            else return BadRequest();
        }
    }
}
