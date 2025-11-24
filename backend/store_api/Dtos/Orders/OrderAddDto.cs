using store_api.Entities;
using store_api.Utils;

namespace store_api.Dtos.Orders;

public class OrderAddDto
{   
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemEntity> OrderItems { get; set; }

    public OrderAddDto(Guid userId, DateTime createdAt, decimal total, OrderStatus status, List<OrderItemEntity> orderItems)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        CreatedAt = createdAt;
        Total = total;
        Status = status;
        OrderItems = orderItems;
    }
}