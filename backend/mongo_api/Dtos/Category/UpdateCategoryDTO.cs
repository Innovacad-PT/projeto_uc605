using mongo_api.Entities;

namespace mongo_api.Dtos.Category;

public class UpdateCategoryDTO(string? name)
{
    public string? Name { get; set; } = name;

    public CategoryEntity ToEntity(Guid id)
    {
        return new(id, Name);
    }
}