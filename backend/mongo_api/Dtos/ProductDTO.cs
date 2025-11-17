using mongo_api.Entities;

namespace mongo_api.Dtos;

public class ProductDTO(Guid id, string name, string details, string techInfo, double price)
{
    public Guid Id { get; set; } = id;
    public string Name { get; set; } = name;
    public string Details {get; set;} = details;
    public string TechInfo {get; set;} = techInfo;
    public double Price { get; set; } = price;

    public ProductEntity ToProductEntity()
    {
        return new ProductEntity(Id, Name, Details, TechInfo, Price);
    }
}