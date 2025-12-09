using mongo_api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Dtos.Brands;

public class CreateBrandDTO(string name)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;

    public BrandEntity? ToEntity()
    {
        return new(Id, Name);
    }
}