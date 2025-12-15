using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Entities;

public class ProductEntity(Guid id, string name, string? description, double price, int? stock, string? imageUrl,
    Guid? categoryId, Guid? brandId, List<Guid>? technicalSpecs, List<Guid>? reviews,
    DateTime? createdAt, DateTime? updatedAt) : IBaseEntity
{
    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string? Description {get; set;} = description;
    public double Price { get; set; } = price;
    public int? Stock { get; set; }  = stock;
    public string? ImageUrl { get; set; } = imageUrl;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? CategoryId { get; set; } = categoryId;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? BrandId { get; set; } = brandId;
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public List<Guid>? TechnicalSpecs {get; set;} = technicalSpecs;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public List<Guid>? Reviews { get; set; } = reviews;
    public DateTime? CreatedAt { get; set; } = createdAt;
    public DateTime? UpdatedAt { get; set; } = updatedAt;
}