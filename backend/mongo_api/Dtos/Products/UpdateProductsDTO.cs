using mongo_api.Entities;

namespace mongo_api.Dtos;

public class UpdateProductsDTO(string name,  string description, double price,
    int stock, Guid categoryId, Guid brandId, List<ReviewEntity> reviews,
    List<TechnicalSpecEntity> specs, string img)
{
    public string? Name = name;
    public string? Description = description;
    public double? Price = price;
    public int? Stock = stock;
    public string? Img = img;
    public Guid? CategoryId = categoryId;
    public Guid? BrandId = brandId;
    public List<TechnicalSpecEntity>? Specs = specs;
    public List<ReviewEntity>? Reviews = reviews;

}