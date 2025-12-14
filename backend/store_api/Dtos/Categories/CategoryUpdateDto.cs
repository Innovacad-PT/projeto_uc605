using store_api.Entities;

namespace store_api.Dtos.Categories;

public class CategoryUpdateDto<T> : IBaseDto<CategoryEntity>
{
    public string? Name { get; set; }

    public CategoryUpdateDto(){}

    public CategoryUpdateDto(String? name)
    {
        Name = name;
    }
}