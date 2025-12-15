using store_api.Utils;

namespace store_api.Entities;

public class OrderEntity(Guid id, Guid userId, long createdAt, Decimal total, String status, List<OrderItemEntity> orderItems)
{
    public Guid Id { get; set; } = id;
    public Guid UserId { get; set; } = userId;
    public long CreatedAt { get; set; } = createdAt;
    public Decimal Total { get; set; } = total;
    public String Status { get; set; } = status;
    public List<OrderItemEntity> OrderItems { get; set; } = orderItems;
}