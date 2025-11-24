using store_api.Entities;

namespace store_api.Dtos.Categories;

public class CategoryAddDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }

    public CategoryAddDto(String name)
    {
        Id = Guid.NewGuid();
        Name = name;
    }

    public CategoryEntity ToEntity()
    {
        return new CategoryEntity(Id, Name);
    }
}