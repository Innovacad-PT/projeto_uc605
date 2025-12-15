using mongo_api.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;
namespace mongo_api.Dtos;

public class CreateProductDTO(string name, string? description, double price, int? stock,
    string? imageUrl, Guid? categoryId, Guid? brandId, List<Guid>? technicalSpecs,
    List<Guid>? reviews)
{
    private static DateTime _date = DateTime.UtcNow;

    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public double Price { get; set; } = price;
    public int? Stock { get; set; } = stock;
    public string? ImageUrl { get; set; } = imageUrl;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? CategoryId { get; set; } = categoryId;
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public Guid? BrandId { get; set; } = brandId;
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public List<Guid>? TechnicalSpecs { get; set; } = technicalSpecs ?? new List<Guid>();
    
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    public List<Guid>? Reviews { get; set; } = reviews ?? new List<Guid>();
    public DateTime? CreatedAt { get; set; } = _date;
    public DateTime? UpdatedAt { get; set; } = _date;

    public ProductEntity ToEntity()
    {
        return new(Id, Name, Description, Price, Stock, ImageUrl, CategoryId, BrandId, TechnicalSpecs, Reviews, CreatedAt, UpdatedAt);
    }
}