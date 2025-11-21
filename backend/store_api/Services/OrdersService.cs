using store_api.Entities;
using store_api.Repositories;

namespace store_api.Services;

public class OrdersService
{
    
    private OrdersRepository _repository = new OrdersRepository();

    public OrderEntity? CreateOrder(Guid userId, List<OrderItemEntity> cartItems)
    {
        throw new NotImplementedException();
    }

    public OrderEntity? GetOrder(Guid id)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<OrderEntity> GetUserOrders(Guid userId)
    {
        throw new NotImplementedException();
    }

    public decimal CalculateOrderTotal(Guid orderId)
    {
        throw new NotImplementedException();
    }
}