using mongo_api.Entities;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace mongo_api.Dtos;

public class UpdateProductsDTO
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public int? Stock { get; set; }
    public string? ImageUrl { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public List<Guid>? TechnicalSpecs { get; set; }
    public List<Guid>? Reviews { get; set; }

}