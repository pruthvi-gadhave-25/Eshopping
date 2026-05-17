using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderService.Service;
using Microsoft.AspNetCore.Authorization;
using OrderService.DTO;
using System.Security.Claims;

namespace OrderService.Controller
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderDto dto)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (userId == null)
            {
                return Unauthorized();
            }

            var result = await _orderService
                .CreateOrder(dto, int.Parse(userId));

            if (!result)
            {
                return BadRequest("Order creation failed");
            }

            return Ok("Order created successfully");
        }

    }
}
