using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class CategoriesRepository : IBaseRepository<CategoryEntity>
{
    public Result<CategoryEntity> Add(IBaseDto<CategoryEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<CategoryEntity> Update(Guid id, IBaseDto<CategoryEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<CategoryEntity> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<CategoryEntity> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Result<CategoryEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<CategoryEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Result<CategoryEntity> GetByName(string name)
    {
        throw new NotImplementedException();
    }
}