using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/products")]
public class ProductController(MongoRepository repository) : Controller
{
    private readonly MongoRepository _mongoRepository = repository;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _mongoRepository.GetProducts();
        if (products.Count == 0) return NotFound();

        return Ok(Json(products));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var product = await _mongoRepository.GetProduct(id);
        if (product == null) return NotFound();

        return Ok(Json(product));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] ProductDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newProduct = await _mongoRepository.CreateProduct(dto);
        if (newProduct == null) return NotFound();

        return Ok(Json(newProduct));
    }

    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] ProductDTO dto)
    {
        var updatedProduct = await _mongoRepository.UpdateProduct(dto);
        if (updatedProduct == null) return NotFound();

        return Ok(Json(updatedProduct));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var oldProduct = await _mongoRepository.DeleteProduct(id);
        if (oldProduct == null) return NotFound();

        return Ok(Json(oldProduct));
    }
}