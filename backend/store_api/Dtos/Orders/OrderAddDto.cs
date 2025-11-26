using store_api.Entities;
using store_api.Utils;

namespace store_api.Dtos.Orders;

public class OrderAddDto(Guid userId, DateTime createdAt, decimal total, OrderStatus status, List<OrderItemEntity> orderItems)
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public Guid UserId { get; set; } = userId;
    public DateTime CreatedAt { get; set; } = createdAt;
    public Decimal Total { get; set; } = total;
    public OrderStatus Status { get; set; } = status;
    public List<OrderItemEntity> OrderItems { get; set; } = orderItems;

    public OrderEntity ToEntity()
    {
        return new (
            Id,
            UserId,
            CreatedAt,
            Total,
            Status,
            OrderItems
            );
    }
}