using store_api.Dtos;
using store_api.Entities;

public class ProductCreateDto
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid CategoryId  { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public decimal Price { get; set; }
    public string? Details { get; set; }
    public List<Guid>? TechnicalSpecs { get; set; }
    public IFormFile? ImageFile { get; set; }
}