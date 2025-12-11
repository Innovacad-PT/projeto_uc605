using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Entities;

public class OrderEntity(Guid id, Guid userId, DateTime createdAt, double? total, string? status, List<OrderItemEntity>? orderItemEntity) : IBaseEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; } = userId;
    public DateTime CreatedAt { get; set; } = createdAt;
    public double? Total { get; set; } = total;
    public string? Status { get; set; } = status;
    public  List<OrderItemEntity>? OrderItems  { get; set; } = orderItemEntity;
}