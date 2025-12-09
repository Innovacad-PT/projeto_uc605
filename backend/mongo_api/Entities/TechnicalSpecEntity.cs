using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;
namespace mongo_api.Entities;

public class TechnicalSpecEntity(Guid id, string name, string value)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Value { get; set; } = value;
}