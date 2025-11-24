namespace store_api.Dtos.Categories;

public class CategoryUpdateDto
{
    public string? Name { get; set; }

    public CategoryUpdateDto(String? name)
    {
        Name = name;
    }
}