using store_api.Dtos.Categories;
using store_api.Entities;
using store_api.Repositories;
using store_api.Utils;

namespace store_api.Services;

public class CategoriesService
{
    
    private readonly CategoriesRepository _categoriesRepository = new();

    public Result<CategoryEntity> Add(CategoryAddDto dto)
    {
        
        Result<CategoryEntity> result = _categoriesRepository.GetById(dto.Id);

        if (result is Success<CategoryEntity>)
        {
            return new Failure<CategoryEntity>(ResultCode.CATEGORY_NOT_CREATED, $"The category with id ({dto.Id})) could not be created");
        }
        
        Result<CategoryEntity> addResult = _categoriesRepository.Add(new CategoryEntity(dto.Id, dto.Name));
        
        return addResult;
    }

    public Result<CategoryEntity?> GetById(Guid id)
    {
        return _categoriesRepository.GetById(id);
    }
    
    public Result<CategoryEntity?> CreateCategory(CategoryAddDto dto)
    {
        Result<CategoryEntity?> result = _categoriesRepository.Add(new(dto.Id, dto.Name));
        
        return result;
    }

    public Result<IEnumerable<CategoryEntity>?> GetAllCategories()
    {
        Result<IEnumerable<CategoryEntity>?> result = _categoriesRepository.GetAll();
        return result;
    }
}