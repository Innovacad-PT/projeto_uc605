using mongo_api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Dtos.Discounts;

public class CreateDiscountDTO(double percentage, DateTime startTime, DateTime endTime)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    public double Percentage { get; set; } = percentage;
    public DateTime StartTime { get; set; } = startTime;
    public DateTime EndTime { get; set; } = endTime;

    public DiscountEntity? ToEntity()
    {
        return new(Id, Percentage, StartTime, EndTime);
    }
}