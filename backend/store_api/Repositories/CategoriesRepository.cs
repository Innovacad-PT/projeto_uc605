using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class CategoriesRepository : IBaseRepository<CategoryEntity>
{
    
    private readonly static List<CategoryEntity> _categories = new();
    
    public Result<CategoryEntity> Add(CategoryEntity entity)
    {
        _categories.Add(entity);
        
        return new Success<CategoryEntity>(ResultCode.CATEGORY_CREATED, "Category created", entity);
    }

    public Result<CategoryEntity> Update(Guid id, IBaseDto<CategoryEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<CategoryEntity> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<CategoryEntity> GetById(Guid id)
    {
        if (!_categories.Any(x => x.Id == id))
        {
            return new Failure<CategoryEntity>(ResultCode.CATEGORY_NOT_FOUND, $"Category with id ({id}) not found");
        }

        return new Success<CategoryEntity>(ResultCode.CATEGORY_FOUND, "Category found", _categories.First(x => x.Id == id));
    }

    public Result<IEnumerable<CategoryEntity>> GetAll()
    {
        if (!_categories.Any())
        {
            return new Failure<IEnumerable<CategoryEntity>>(ResultCode.CATEGORY_NOT_FOUND, $"Categories not found");
        }

        return new Success<IEnumerable<CategoryEntity>>(ResultCode.CATEGORY_FOUND, "Category found", _categories);
    }

    public Result<CategoryEntity> GetByName(string name)
    {
        throw new NotImplementedException();
    }
}