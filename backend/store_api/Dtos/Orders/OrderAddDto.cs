using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Dtos.Orders;

public class OrderAddDto(Guid userId, decimal total, OrderStatus status, Dictionary<Guid, int> products)
{
    public Guid UserId { get; set; } = userId;
    public Decimal Total { get; set; } = total;
    public OrderStatus Status { get; set; } = status;
    public Dictionary<Guid, int> Products { get; set; } = products;

    public OrderEntity ToEntity()
    {
        ProductsService _service = new();
        var orderItemEntities = Products.Select(item =>
        {
            
            Result<ProductEntity> pe = _service.GetProductById(item.Key);

            if (pe is Failure<ProductEntity> peFail)
            {
                return null;
            }
            
            ProductEntity product = (pe as Success<ProductEntity>).Value;

            return new OrderItemEntity(
                0,
                product.Id,
                item.Value,
                product.Price
            );
        }).ToList();
        
        return new (
            (int)new Random().NextInt64(),
            UserId,
            DateTime.Now,
            Total,
            Status,
            orderItemEntities
            );
    }
}