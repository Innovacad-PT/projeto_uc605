using store_api.Dtos.Products;

namespace store_api.Entities;

public class ProductEntity
{
    public Guid? Id {get; set;}
    public string Name {get; set;}
    public double Price { get; set; }
    public string Details {get; set;}
    public string TechInfo {get; set;}

    public ProductEntity(Guid id, string name, double price, string details, string techInfo)
    {
        Id = id;
        Name = name;
        Price = price;
        Details = details;
        TechInfo = techInfo;
    }

    public void Update(ProductUpdateDto product)
    {
        Name = product.Name ?? Name;
        Price = product.Price ?? Price;
        Details = product.Details ?? Details;
        TechInfo = product.TechInfo ?? TechInfo;
    }
}