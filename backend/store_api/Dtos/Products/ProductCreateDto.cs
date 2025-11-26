using store_api.Entities;

public class ProductCreateDto
{
    public Guid Id { get; set; }
    public Guid BrandId { get; set; }
    public Guid CategoryId  { get; set; }
    public string Name { get; set; }
    public int Stock { get; set; }
    public double Price { get; set; }
    public string? Details { get; set; }
    public List<TechnicalSpecsEntity>? TechnicalSpecs { get; set; }

    public IFormFile? Image { get; set; }
}