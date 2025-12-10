using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos.Discounts;
using mongo_api.Repositories;
using mongo_api.Utils;

namespace mongo_api.Controllers;

[ApiController]
[Route("/discounts")]
public class DiscountController(MongoRepository repository, Redis redis) : Controller
{
    private readonly DiscountRepository _repository = repository.DiscountRepo;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var discounts = await redis.GetOrSetCache("discounts", async () => await _repository.GetAll());
        if (discounts.Count == 0) return NotFound();

        return Ok(Json(discounts));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var discount = await redis.GetOrSetCache($"discount:{id}", async () => (await _repository.GetById(id))!);
        if (discount == null) return NotFound();

        return Ok(Json(discount));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateDiscountDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newDiscount = await _repository.Create(dto);
        if (newDiscount == null) return NotFound();

        return Ok(Json(newDiscount));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldDiscount = await _repository.Delete(id);
        if (oldDiscount == null) return NotFound();

        return Ok(Json(oldDiscount));
    }
}