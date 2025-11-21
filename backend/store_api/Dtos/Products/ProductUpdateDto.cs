using store_api.Dtos.TechnicalSpecs;
using store_api.Entities;

public class ProductUpdateDto
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public double? Price { get; set; }
    public int? Stock { get; set; }

    public Guid? CategoryId { get; set; }
    
    public List<TechnicalSpecsUpdateDto>? TechnicalSpecs { get; set; }

    public IFormFile? ImageFile { get; set; }
}