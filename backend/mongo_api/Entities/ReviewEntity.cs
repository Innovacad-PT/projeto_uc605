using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
namespace mongo_api.Entities;

public class ReviewEntity(Guid id, Guid userId, Guid productId, double rating,
    string? comment, DateTime createdAt) : IBaseEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; } = userId;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ProductId { get; set; } = productId;
    public double Rating { get; set; } = rating;
    public string? Comment { get; set; } = comment;
    public DateTime CreatedAt { get; set; } = createdAt;
}