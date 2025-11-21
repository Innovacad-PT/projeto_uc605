public class ProductCreateDto
{
    public string Name { get; set; }
    public double Price { get; set; }
    public string[]? Categories  { get; set; }
    public string? Details { get; set; }
    public string? TechInfo { get; set; }

    public IFormFile? Image { get; set; }
}