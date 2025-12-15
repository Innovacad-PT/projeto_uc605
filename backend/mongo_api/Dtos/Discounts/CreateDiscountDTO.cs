using mongo_api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Dtos.Discounts;

public class CreateDiscountDTO
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? ProductId { get; set; }
    public double? Percentage { get; set; }
    public long? StartDate { get; set; }
    public long? EndDate { get; set; }

    public DiscountEntity ToEntity()
    {
        return new(Id, ProductId, Percentage, StartDate, EndDate);
    }
}