using mongo_api.Entities;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Swashbuckle.AspNetCore.Annotations;
namespace mongo_api.Dtos;

public class CreateProductDTO(string name, string description, double price, int stock,
    string imageUrl, BrandEntity brand, List<TechnicalSpecEntity> technicalSpecs,
    List<ReviewEntity> reviews, DiscountEntity discount)
{
    private static DateTime _date = DateTime.UtcNow;

    [BsonId]
    [BsonGuidRepresentation(GuidRepresentation.Standard)]
    [SwaggerIgnore]
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = name;
    public string Description { get; set; } = description;
    public double Price = price;
    public int Stock = stock;
    public string ImageUrl { get; set; } = imageUrl;
    public BrandEntity Brand { get; set; } = brand;
    public List<TechnicalSpecEntity> TechnicalSpecs { get; set; } = technicalSpecs;
    public List<ReviewEntity> Reviews { get; set; } = reviews;
    public DateTime CreatedAt { get; set; } = _date;
    public DateTime UpdatedAt { get; set; } = _date;
    public DiscountEntity Discount { get; set; } = discount;

    public ProductEntity ToEntity()
    {
        return new(Id, Name, Description, Price, Stock, ImageUrl, Brand, TechnicalSpecs, Reviews, CreatedAt, UpdatedAt, Discount);
    }
}