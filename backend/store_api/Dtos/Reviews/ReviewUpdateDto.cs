namespace store_api.Dtos.Reviews;

public class ReviewUpdateDto
{
    public int? Rating { get; set; }
    public string? Comment { get; set; }

    public ReviewUpdateDto(int? rating, string? comment)
    {
        Rating = Rating;
        Comment = comment;
    }
    
}