using OrderService.DTO;

namespace OrderService.Service
{
    public interface IOrderService
    {
        Task<bool>CreateOrder(CreateOrderDto dto ,int userId);
    }
}
