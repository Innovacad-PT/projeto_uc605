using System.Collections;
using store_api.Dtos;
using store_api.Dtos.TechnicalSpecs;
using store_api.Entities;

public class ProductUpdateDto<T> : IBaseDto<ProductEntity>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public int? Stock { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public List<ReviewEntity>? Reviews { get; set; }
    public List<Guid>? TechnicalSpecs { get; set; }
    public IFormFile? ImageFile { get; set; }

    public ProductUpdateDto()
    {
        
    }

    public ProductUpdateDto(string? name, string? description, decimal? price,
        int? stock, Guid? categoryId, Guid? brandId, List<ReviewEntity>? reviews,
        List<Guid>? technicalSpecs, IFormFile? imageFile)
    {
        Name = name;
        Description = description;
        Price = price;
        Stock = stock;
        CategoryId = categoryId;
        BrandId = brandId;
        Reviews = reviews;
        TechnicalSpecs = technicalSpecs;
        ImageFile = imageFile;
    }
}