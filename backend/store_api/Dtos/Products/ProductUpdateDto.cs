using System.Collections;
using store_api.Dtos;
using store_api.Dtos.TechnicalSpecs;
using store_api.Entities;

public class ProductUpdateDto<T>(string? name, string? description, decimal? price,
    int? stock, Guid? categoryId, Guid? brandId, List<ReviewEntity>? reviews,
    List<ProductTechnicalSpecsEntity>? technicalSpecs, string? imageUrl) : IBaseDto<ProductEntity>
{
    public string? Name { get; set; } = name;
    public string? Description { get; set; } = description;
    public decimal? Price { get; set; } = price;
    public int? Stock { get; set; } = stock;
    public Guid? CategoryId { get; set; } = categoryId;
    public Guid? BrandId { get; set; } = brandId;
    public List<ReviewEntity>? Reviews { get; set; } = reviews;
    public List<ProductTechnicalSpecsEntity>? TechnicalSpecs { get; set; } = technicalSpecs;
    public string? ImageUrl { get; set; } = imageUrl;
    public IFormFile? ImageFile { get; set; } = null;
}