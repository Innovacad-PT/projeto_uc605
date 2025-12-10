using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos;
using mongo_api.Repositories;
using mongo_api.Utils;

namespace mongo_api.Controllers;

[ApiController]
[Route("/products")]
public class ProductController(MongoRepository repository, Redis redis) : Controller
{
    private readonly ProductRepository _repository = repository.ProductRepo;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var products = await redis.GetOrSetCache("products", async () => await _repository.GetAll());
        if (products.Count == 0) return NotFound();

        return Ok(Json(products));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var product = await redis.GetOrSetCache($"product:{id}", async () => (await _repository.GetById(id))!);
        if (product == null) return NotFound();

        return Ok(Json(product));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
    {
        var newProduct = await _repository.Create(dto);
        if (newProduct == null) return NotFound();

        return Ok(Json(newProduct));
    }
    
    /*[HttpPut]
    public async Task<IActionResult> Update([FromBody] ProductDTO dto)
    {
        var updatedProduct = await _repository.Update(dto);
        if (updatedProduct == null) return NotFound();

        return Ok(Json(updatedProduct));
    }*/

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldProduct = await _repository.Delete(id);
        if (oldProduct == null) return NotFound();

        return Ok(Json(oldProduct));
    }
}