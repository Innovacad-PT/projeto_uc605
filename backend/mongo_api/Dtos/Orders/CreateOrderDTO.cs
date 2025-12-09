using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

using mongo_api.Entities;

namespace mongo_api.Dtos.Orders;

public class CreateOrderDTO(Guid userId, double total, string status, List<ProductEntity> products)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; } = userId;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public double Total { get; set; } = total;
    public string Status { get; set; } = status;
    public  List<ProductEntity> Products { get; set; } = products;

    public OrderEntity ToEntity()
    {
        return new(Id, UserId, CreatedAt, Total, Status, Products);
    }
}