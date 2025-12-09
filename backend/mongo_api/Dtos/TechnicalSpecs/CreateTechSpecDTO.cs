using mongo_api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Dtos.TechnicalSpecs;

public class CreateTechSpecDTO(string name, string? value)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string? Value { get; set; } = value;

    public TechnicalSpecEntity ToEntity()
    {
        return new(Id, Name, Value);
    }
}