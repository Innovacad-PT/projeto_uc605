using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Utils;

namespace store_api.Repositories;

public class ReviewsRepository : IBaseRepository<ReviewEntity>
{
    public Result<ReviewEntity> Add(ReviewEntity entity)
    {
        throw new NotImplementedException();
    }

    public Result<ReviewEntity> Update(Guid id, IBaseDto<ReviewEntity> entity)
    {
        throw new NotImplementedException();
    }

    public Result<ReviewEntity> Delete(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<ReviewEntity> GetById(int id)
    {
        throw new NotImplementedException();
    }

    public Result<ReviewEntity> GetById(Guid id)
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<ReviewEntity>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Result<IEnumerable<ReviewEntity>> GetReviewsByProduct(Guid productId)
    {
        throw new NotImplementedException();
    }

    public Result<ReviewEntity> GetUserReview(Guid userId, Guid productId)
    {
        throw new NotImplementedException();
    }
}