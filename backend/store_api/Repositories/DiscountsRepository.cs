using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class DiscountsRepository : IBaseRepository<DiscountEntity>
{
    public Result<DiscountEntity> Add(IBaseDto<DiscountEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<DiscountEntity> Update(Guid id, IBaseDto<DiscountEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<DiscountEntity> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<DiscountEntity> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Result<DiscountEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<DiscountEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Result<DiscountEntity> GetActiveDiscount(Guid productId)
    {
        throw new NotImplementedException();
    }
}