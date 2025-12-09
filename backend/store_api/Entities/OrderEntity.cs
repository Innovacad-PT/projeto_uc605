using store_api.Utils;

namespace store_api.Entities;

public class OrderEntity(int id, Guid userId, DateTime createdAt, Decimal total, OrderStatus status, List<OrderItemEntity> orderItems)
{
    public int Id { get; set; } = id;
    public Guid UserId { get; set; } = userId;
    public DateTime CreatedAt { get; set; } = createdAt;
    public Decimal Total { get; set; } = total;
    public OrderStatus Status { get; set; } = status;
    public List<OrderItemEntity> OrderItems { get; set; } = orderItems;
}