using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Entities;

public class ProductEntity(Guid id, string name, string details, string techInfo, double price)
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Details {get; set;} = details;
    public string TechInfo {get; set;} = techInfo;
    public double Price { get; set; } = price;
}