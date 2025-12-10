using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos.Brands;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/brands")]
public class BrandController(MongoRepository repository) : Controller
{
    private readonly BrandRepository _repository = repository.BrandRepo;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var brands = await _repository.GetAll();
        if (brands.Count == 0) return NotFound();

        return Ok(Json(brands));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var brand = await _repository.GetById(id);
        if (brand == null) return NotFound();

        return Ok(Json(brand));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBrandDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newBrand = await _repository.Create(dto);
        if (newBrand == null) return NotFound();

        return Ok(Json(newBrand));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldBrand = await _repository.Delete(id);
        if (oldBrand == null) return NotFound();

        return Ok(Json(oldBrand));
    }
}