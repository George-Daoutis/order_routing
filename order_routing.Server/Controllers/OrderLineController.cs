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
        private readonly IProductService _productService;
        public OrderLineController(IOrderLineService orderLineService, IProductService productService)
        {
            _orderLineService = orderLineService;
            _productService = productService;
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

        [HttpGet("getproducts")]
        public async Task<ActionResult<OrderLineGetDTO>> GetAllProducts()
        {
            var response = await _productService.GetAllProducts();
            if (response != null)
            {
                return Ok(response);
            }
            else return BadRequest();
        }

        [HttpPost("orderff")]
        public async Task<ActionResult<OrderLineGetDTO>> CreateOrderFulfillment([FromBody] OrderlineFulfillmentCreateDTO fulfillmentDTO)
        {
            var response = await _orderLineService.AddOrderFulfillment(fulfillmentDTO);
            if (response != null)
            {
                return Ok(response);
            }
            else return BadRequest();
        }
    }
}
