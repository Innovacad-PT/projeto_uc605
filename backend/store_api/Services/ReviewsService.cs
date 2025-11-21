using store_api.Dtos.Reviews;
using store_api.Entities;

namespace store_api.Services;

public class ReviewsService
{
    public ReviewEntity? AddReview(ReviewAddDto dto)
    {
        throw new NotImplementedException();
    }

    public IEnumerable<ReviewEntity> GetReviews(Guid productId)
    {
        throw new NotImplementedException();
    }

    public ReviewEntity? DeleteReview(Guid reviewId)
    {
        throw new NotImplementedException();
    }
}