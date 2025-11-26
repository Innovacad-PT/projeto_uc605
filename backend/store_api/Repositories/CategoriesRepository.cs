using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Categories;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class CategoriesRepository : IBaseRepository<CategoryEntity>
{
    
    private readonly static List<CategoryEntity> _categories = [
        new (Guid.Parse("40c9354a-1002-425d-a561-45895910ad86"), "Computador")
    ];
    
    public CategoryEntity? Add(CategoryEntity entity)
    {
        if (_categories.Any(c => c.Id == entity.Id))
        {
            throw new SameIdException("Category with the same id already exists.");
        }

        if (_categories.Any(c => c.Name == entity.Name))
        {
            throw new SameNameException("Category with the same name already exists.");
        }
        
        _categories.Add(entity);
        
        return entity;
    }

    public CategoryEntity Update(Guid id, IBaseDto<CategoryEntity> dto)
    {
        CategoryUpdateDto updateDto = dto as CategoryUpdateDto;
        if(updateDto == null)
        {
            throw new InvalidDtoType("Invalid data transfer object type.");
        }
        
        if (!_categories.Any(c => c.Id == id))
        {
            return null;
        }
        
        CategoryEntity category = _categories.First(c => c.Id == id);
        category.Name = updateDto.Name ?? category.Name;
        
        return category;
    }

    public CategoryEntity Delete(Guid id)
    {
        if (!_categories.Any(c => c.Id == id))
        {
            return null;
        }
        
        CategoryEntity category = _categories.First(c => c.Id == id);
        _categories.Remove(category);
        return category;
    }

    public CategoryEntity? GetById(Guid id)
    {
        CategoryEntity? category = _categories.FirstOrDefault(c => c.Id == id);

        return category;
    }

    public IEnumerable<CategoryEntity>? GetAll()
    {
        return _categories;
    }

    public CategoryEntity? GetByName(string name)
    {
        CategoryEntity? category = _categories.FirstOrDefault(c => c.Name == name);
        
        return category;
    }
}