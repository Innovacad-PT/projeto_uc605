using store_api.Controllers;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class BrandsRepository : IBaseRepository<BrandEntity>
{
    public Result<BrandEntity> Add(BrandEntity entity)
    {
        throw new NotImplementedException();
    }

    public Result<BrandEntity> Update(BrandEntity entity)
    {
        throw new NotImplementedException();
    }

    public Result<BrandEntity> Delete(BrandEntity entity)
    {
        throw new NotImplementedException();
    }

    public Result<BrandEntity> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Result<BrandEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<BrandEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Result<BrandEntity> GetByName(String name)
    {
        throw new NotImplementedException();
    }
}