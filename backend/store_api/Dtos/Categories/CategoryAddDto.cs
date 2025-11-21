using store_api.Entities;

namespace store_api.Dtos.Categories;

public class CategoryAddDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    List<ProductEntity> Products { get; set; }

    public CategoryAddDto(String name, String description, List<ProductEntity>? products)
    {
        Id = Guid.NewGuid();
        Name = name;
        Description = description;
        Products = products ?? [];
    } 
}