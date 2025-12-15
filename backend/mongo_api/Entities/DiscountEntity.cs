using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
namespace mongo_api.Entities;

public class DiscountEntity(Guid id, Guid? productId, double? percentage, long? startTime, long? endTime) : IBaseEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? ProductId { get; set; } = productId;
    public double? Percentage { get; set; } = percentage;
    public long? StartDate { get; set; } = startTime;
    public long? EndDate { get; set; } = endTime;
}