using store_api.Dtos;
using store_api.Dtos.TechnicalSpecs;
using store_api.Entities;

public class ProductUpdateDto : IBaseDto<ProductEntity>
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public int? Stock { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? BrandId { get; set; }
    public List<ReviewEntity>? Reviews { get; set; }
    public List<TechnicalSpecsEntity>? TechnicalSpecs { get; set; }

    public IFormFile? ImageFile { get; set; }
}