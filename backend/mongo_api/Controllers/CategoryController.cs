using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos.Category;
using mongo_api.Repositories;
using mongo_api.Utils;

namespace mongo_api.Controllers;

[ApiController]
[Route("/categories")]
public class CategoryController(MongoRepository repository, Redis redis) : Controller
{
    private readonly CategoryRepository _repository = repository.CategoryRepo;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var categories = await redis.GetOrSetCache("categories", async () => await _repository.GetAll());
        if (categories.Count == 0) return NotFound();

        return Ok(Json(categories));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var category = await redis.GetOrSetCache($"category:{id}", async () => (await _repository.GetById(id))!);
        if (category == null) return NotFound();

        return Ok(Json(category));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateCategoryDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newCategory = await _repository.Create(dto);
        if (newCategory == null) return NotFound();

        return Ok(Json(newCategory));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldCategory = await _repository.Delete(id);
        if (oldCategory == null) return NotFound();

        return Ok(Json(oldCategory));
    }
}