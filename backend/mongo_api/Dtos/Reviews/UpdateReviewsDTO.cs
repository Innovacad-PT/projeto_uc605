using mongo_api.Entities;

namespace mongo_api.Dtos.Reviews;

public class UpdateReviewsDTO(int? rating, string? comment)
{
    public int? Rating = rating;
    public string? Comment = comment;

    public ReviewEntity ToEntity(Guid id, Guid userId, Guid productId, DateTime createdAt)
    {
        return new(id, userId, productId, Rating, Comment, createdAt);
    }
}