namespace store_api.Dtos.Products;

public class ProductUpdateDto
{
    
    public string? Name { get; set; }
    public double? Price { get; set; }   
    public string? Description { get; set; }
    public string? Details { get; set; }
    public string? TechInfo { get; set; }

    public ProductUpdateDto(string? Name, double? Price, string? Description, string? Details, string? TechInfo)
    {
        this.Name = Name;
        this.Price = Price;
        this.Description = Description;
        this.Details = Details;
        this.TechInfo = TechInfo;
    }
}