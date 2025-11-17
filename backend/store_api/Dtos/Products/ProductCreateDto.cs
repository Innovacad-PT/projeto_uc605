using store_api.Entities;

namespace store_api.Dtos.Products;

public class ProductCreateDto
{
    
    public Guid? Id {get; set;}
    public string Name {get; set;}
    public double Price { get; set; }
    public string? Details {get; set;}
    public string? TechInfo {get; set;}

    public ProductCreateDto(Guid? id,string name, double price, string? details, string? techInfo)
    {
        if (id == null)
        {
            id =  Guid.NewGuid();
        }

        Id = id;
        Name = name;
        Price = price;
        Details = details;
        TechInfo = techInfo;
    }

    public ProductEntity ToEntity()
    {
        return new(Id!.Value, Name, Price, Details ?? "", TechInfo ?? "");
    }
    
}