using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos.Brands;
using mongo_api.Entities;
using mongo_api.Repositories;
using mongo_api.Utils;

namespace mongo_api.Controllers;

[ApiController]
[Route("/brands")]
public class BrandController(MongoRepository repository, Redis redis) : Controller
{
    private readonly BrandRepository _repository = repository.BrandRepo;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var brands = await redis.GetOrSetCache("brands", async () => await _repository.GetAll());
        if (brands == null || brands.Count == 0) return NotFound();

        return Ok(brands);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var brand = await redis.GetOrSetCache($"brand:{id}", async () => (await _repository.GetById(id))!);
        if (brand == null) return NotFound();
        
        return Ok(brand);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateBrandDTO dto)
    {
        dto.Id = Guid.NewGuid();

        BrandEntity? newBrand = await _repository.Create(dto);


        if (newBrand == null) return NotFound();

        return Ok(newBrand);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldBrand = await _repository.Delete(id);
        if (oldBrand == null) return NotFound();

        return Ok(oldBrand);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateBrandDTO dto)
    {
        var brand = await _repository.GetById(id);
        if (brand == null) return NotFound();

        var updatedBrand = await _repository.Update(id, dto);
        if (updatedBrand == null) return NotFound();

        return Ok(updatedBrand);
    }
}