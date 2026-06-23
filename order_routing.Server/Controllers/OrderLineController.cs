using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using order_routing.Server.Models;
using order_routing.Server.Services;
using System.Security.Claims;

namespace order_routing.Server.Controllers
{
    [Authorize]
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
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStoreId = int.Parse(User.FindFirst("UserStoreId")?.Value ?? "0");

            //if (userRole == "StoreUser" && userStoreId != storeId)
            //{
            //    return Forbid();
            //}

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
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;
            var userStoreId = int.Parse(User.FindFirst("UserStoreId")?.Value ?? "0");

            if (userRole == "StoreUser")
            {
                return Unauthorized();
            }

            var response = await _orderLineService.GetAllOrderLines();
            if (response != null)
            {
                return Ok(response);
            }
            else return BadRequest();
        }

        [HttpGet("getproducts")]
        public async Task<ActionResult<List<OrderLineGetDTO>>> GetAllProducts()
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
