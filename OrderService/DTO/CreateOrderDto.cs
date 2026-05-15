using OrderService.Models;

namespace OrderService.DTO
{
    public class CreateOrderDto
    {
        public List<OrderItem> Items { get; set; }
    }
}
