using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/products")]
public class ProductController(MongoRepository repository) : Controller
{
    private readonly ProductRepository _repository = repository.ProductRepo;

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        var products = await _repository.GetProducts();
        if (products.Count == 0) return NotFound();

        return Ok(Json(products));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProduct([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var product = await _repository.GetProduct(id);
        if (product == null) return NotFound();

        return Ok(Json(product));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateProductDTO dto)
    {
        var newProduct = await _repository.CreateProduct(dto);
        if (newProduct == null) return NotFound();

        return Ok(Json(newProduct));
    }
/*
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] ProductDTO dto)
    {
        var updatedProduct = await _repository.UpdateProduct(dto);
        if (updatedProduct == null) return NotFound();

        return Ok(Json(updatedProduct));
    }*/

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var oldProduct = await _repository.DeleteProduct(id);
        if (oldProduct == null) return NotFound();

        return Ok(Json(oldProduct));
    }
}