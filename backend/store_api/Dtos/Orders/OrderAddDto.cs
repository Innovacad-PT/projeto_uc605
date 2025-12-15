using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Dtos.Orders;

public class OrderAddDto(Guid userId, decimal total, String status, Dictionary<Guid, int> products)
{
    public Guid UserId { get; set; } = userId;
    public Decimal Total { get; set; } = total;
    
    public String Status { get; set; } = status;
    public Dictionary<Guid, int> Products { get; set; } = products;
    
}