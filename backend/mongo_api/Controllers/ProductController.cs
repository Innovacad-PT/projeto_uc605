using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos;
using mongo_api.Repositories;
using mongo_api.Utils;
using Swashbuckle.AspNetCore.Annotations;

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
        if (products?.Count == 0) return NotFound();

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var product = await redis.GetOrSetCache($"product:{id}", async () => (await _repository.GetById(id))!);
        if (product == null) return NotFound();

        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
    {
        var newProduct = await _repository.Create(dto);
        if (newProduct == null) return NotFound();

        return Ok(newProduct);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateProductsDTO dto)
    {
        
        
        var discount = await _repository.GetById(id);
        if (discount == null) return NotFound();

        var updatedDiscount = await _repository.Update(id, dto);
        if (updatedDiscount == null) return NotFound();

        return Ok(updatedDiscount);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldProduct = await _repository.Delete(id);
        if (oldProduct == null) return NotFound();

        return Ok(oldProduct);
    }
}