using Microsoft.EntityFrameworkCore;
using OrderService.Data;
using OrderService.DTO;
using OrderService.Models;
using System.Security.Claims;

namespace OrderService.Service
{
    public class OrdersService :IOrderService
    {

        private readonly OrderDbContext _orderDbContext;

        public OrdersService(OrderDbContext orderDbContext)
        {
            _orderDbContext = orderDbContext;
        }

        public async Task<bool> CreateOrder(CreateOrderDto dto ,int userId )
        {           
            try
            {
                
                var order = new Order
                {
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,

                    TotalAmount = dto.Items.Sum(x =>
                        x.Price * x.Quantity),

                    Items = dto.Items.Select(x => new OrderItem
                    {
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        Price = x.Price,
                        Quantity = x.Quantity
                    }).ToList()
                };

                _orderDbContext.Orders.Add(order);

                await _orderDbContext.SaveChangesAsync();

                return true;


            }
            catch (Exception ex)
            {

               Console.WriteLine(ex.Message);
                return false;                
            }            
        }
    }
}
