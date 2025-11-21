using store_api.Utils;

namespace store_api.Entities;

public class OrderEntity
{
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public DateTime CreatedAt { get; set; }
    public Decimal Total { get; set; }
    public OrderStatus Status { get; set; }
    public List<OrderItemEntity> OrderItems { get; set; }
}