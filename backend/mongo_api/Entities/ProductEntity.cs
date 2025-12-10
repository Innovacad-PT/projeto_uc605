using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Swashbuckle.AspNetCore.Annotations;

namespace mongo_api.Entities;

public class ProductEntity(Guid id, string name, string? description, double price, int? stock, string? imageUrl,
    BrandEntity? brand, List<TechnicalSpecEntity>? technicalSpecs, List<ReviewEntity>? reviews, DateTime? createdAt,
    DateTime? updatedAt, DiscountEntity? discount) : IBaseEntity
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
    public BrandEntity? Brand { get; set; } = brand;
    public List<TechnicalSpecEntity>? TechnicalSpecs {get; set;} = technicalSpecs;
    public List<ReviewEntity>? Reviews { get; set; } = reviews;
    public DateTime? CreatedAt { get; set; } = createdAt;
    public DateTime? UpdatedAt { get; set; } = updatedAt;
    public DiscountEntity? Discount { get; set; } = discount;
}