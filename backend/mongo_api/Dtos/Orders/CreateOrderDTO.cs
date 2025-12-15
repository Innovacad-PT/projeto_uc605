using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

using mongo_api.Entities;

namespace mongo_api.Dtos.Orders;

public class CreateOrderDTO
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; }
    
    [SwaggerIgnore]
    public long CreatedAt { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    public double? Total { get; set; }
    public string? Status { get; set; }
    public List<OrderItemEntity>? OrderItems { get; set; }

    public OrderEntity ToEntity()
    {
        return new(Id, UserId, CreatedAt, Total, Status, OrderItems);
    }
}