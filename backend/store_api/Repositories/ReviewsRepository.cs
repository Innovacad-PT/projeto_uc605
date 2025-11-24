using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Reviews;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class ReviewsRepository : IBaseRepository<ReviewEntity>
{
    
    private readonly static List<ReviewEntity> _reviews = new List<ReviewEntity>();
    
    public ReviewEntity? Add(ReviewEntity entity)
    {
        if (_reviews.Any(r => r.Id == entity.Id))
        {
            throw new SameIdException("The review with the same id already exists");
        }

        if (_reviews.Any(r => r.ProductId == entity.ProductId && r.UserId == entity.UserId))
        {
            return null;
        }
        
        _reviews.Add(entity);
        return entity;
    }

    public ReviewEntity? Update(Guid id, IBaseDto<ReviewEntity> entity)
    {
        ReviewUpdateDto updateDto = entity as ReviewUpdateDto;

        if (updateDto == null)
        {
            throw new InvalidDtoType("Invalid data transfer object type");
        }

        if (!_reviews.Any(r => r.Id == id))
        {
            return null;
        }
        
        ReviewEntity review = _reviews.First(r => r.Id == id);

        review.Rating = updateDto.Rating ?? review.Rating;
        review.Comment = updateDto.Comment ?? review.Comment;

        return review;
    }

    public ReviewEntity? Delete(Guid id)
    {
        if (!_reviews.Any(r => r.Id == id))
        {
            return null;
        }
        
        ReviewEntity review = _reviews.First(r => r.Id == id);
        _reviews.Remove(review);
        return review;
    }

    public ReviewEntity? GetById(Guid id)
    {
        ReviewEntity? review = _reviews.FirstOrDefault(r => r.Id == id);

        return review;
    }

    public IEnumerable<ReviewEntity> GetAll()
    {
        return _reviews;
    }

    public IEnumerable<ReviewEntity>? GetReviewsByProduct(Guid productId)
    {
        IEnumerable<ReviewEntity>? reviews = _reviews.Where(r => r.ProductId == productId);
        return reviews;
    }

    public ReviewEntity? GetUserReview(Guid userId, Guid productId)
    {
        ReviewEntity? review = _reviews.FirstOrDefault(r => r.UserId == userId && r.ProductId == productId);
        return review;
    }
}