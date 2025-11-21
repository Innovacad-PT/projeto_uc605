namespace store_api.Dtos.Reviews;

public class ReviewAddDto
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid ProductId { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public ReviewAddDto(Guid userId, Guid productId, int rating, string comment)
    {
        Id = Guid.NewGuid();
        UserId = userId;
        ProductId = productId;
        Rating = rating;
        Comment = comment;
        CreatedAt = DateTime.Now;
    }
}