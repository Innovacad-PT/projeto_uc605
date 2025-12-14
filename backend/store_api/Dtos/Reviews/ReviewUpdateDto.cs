using store_api.Entities;

namespace store_api.Dtos.Reviews;

public class ReviewUpdateDto<T> : IBaseDto<ReviewEntity>
{
    public int? Rating { get; set; }
    public string? Comment { get; set; }

    public ReviewUpdateDto(int? rating, string? comment)
    {
        Rating = Rating;
        Comment = comment;
    }
    
}