using store_api.Entities;
using store_api.Utils;

namespace store_api.Dtos.Orders;

public class OrderUpdateDto
{
    public Decimal? Total { get; set; }
    public OrderStatus? Status { get; set; }
    public List<OrderItemEntity>? OrderItems { get; set; }

    public OrderUpdateDto(decimal? total, OrderStatus? status, List<OrderItemEntity>? items)
    {
        Total = total;
        Status = status;
        OrderItems = items;
    }
}