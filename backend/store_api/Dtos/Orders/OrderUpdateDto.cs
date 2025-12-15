using store_api.Entities;
using store_api.Utils;

namespace store_api.Dtos.Orders;

public class OrderUpdateDto<T> : IBaseDto<OrderEntity>
{
    public Decimal? Total { get; set; }
    public String? Status { get; set; }
    public List<OrderItemEntity>? OrderItems { get; set; }

    public OrderUpdateDto(decimal? total, String? status, List<OrderItemEntity>? items)
    {
        Total = total;
        Status = status;
        OrderItems = items;
    }
}