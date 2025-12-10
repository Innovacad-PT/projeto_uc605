using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos.Reviews;
using mongo_api.Repositories;
using mongo_api.Utils;

namespace mongo_api.Controllers;

[ApiController]
[Route("/reviews")]
public class ReviewController(MongoRepository repository, Redis redis) : Controller
{
    private readonly ReviewRepository _repository = repository.ReviewRepo;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var reviews = await redis.GetOrSetCache("reviews", async () => await _repository.GetAll());
        if (reviews.Count == 0) return NotFound();

        return Ok(Json(reviews));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var review = await redis.GetOrSetCache($"review:{id}", async () => (await _repository.GetById(id))!);
        if (review == null) return NotFound();

        return Ok(Json(review));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateReviewDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newReview = await _repository.Create(dto);
        if (newReview == null) return NotFound();

        return Ok(Json(newReview));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldReview = await _repository.Delete(id);
        if (oldReview == null) return NotFound();

        return Ok(Json(oldReview));
    }
}