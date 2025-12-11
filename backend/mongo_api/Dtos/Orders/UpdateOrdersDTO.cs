using mongo_api.Entities;

namespace mongo_api.Dtos.Orders;

public class UpdateOrdersDTO(string? status)
{
    public readonly string? Status = status;
}