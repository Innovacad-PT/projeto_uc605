using mongo_api.Entities;

namespace mongo_api.Dtos.Reviews;

public class UpdateReviewsDTO
{
    public int? Rating { get; set; }
    public string? Comment { get; set; }
}