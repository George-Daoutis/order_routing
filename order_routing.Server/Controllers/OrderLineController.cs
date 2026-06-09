using Microsoft.AspNetCore.Mvc;
using order_routing.Server.Models;
using order_routing.Server.Services;

namespace order_routing.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderLineController: ControllerBase
    {
        private readonly IOrderLineService _orderLineService;
        public OrderLineController(IOrderLineService orderLineService)
        {
            _orderLineService = orderLineService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderLineGetDTO>> CreateOrderLine([FromBody] OrderLineCreateDTO orderlineCreateDTO)
        {
            var response = await _orderLineService.NewOrder(orderlineCreateDTO);
            if (response != null)
            {
                return Ok(response);
            }
            else return BadRequest();
        }

        [HttpGet]
        public async Task<ActionResult<OrderLineGetDTO>> GetAllOrderLines()
        {
            var response = await _orderLineService.GetAllOrderLines();
            if (response != null)
            {
                return Ok(response);
            }
            else return BadRequest();
        }
    }
}
