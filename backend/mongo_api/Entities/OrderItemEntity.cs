using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Entities;

public class OrderItemEntity(Guid id, Guid productId, int? quantity, decimal? unitPrice)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ProductId { get; set; } = productId;
    public int? Quantity { get; set; } = quantity;
    public decimal? UnitPrice { get; set; } = unitPrice;
}