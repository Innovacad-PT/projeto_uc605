using mongo_api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Dtos.Reviews;

public class CreateReviewDTO(Guid userId, Guid productId, int? rating, string? comment)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid UserId { get; set; } = userId;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid ProductId { get; set; } = productId;
    public int? Rating { get; set; } = rating;
    public string? Comment { get; set; } = comment;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ReviewEntity ToEntity()
    {
        return new(Id, UserId, ProductId, Rating, Comment, CreatedAt);
    }
}