using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos.TechnicalSpecs;
using mongo_api.Repositories;
using mongo_api.Utils;

namespace mongo_api.Controllers;

[ApiController]
[Route("/techspecs")]
public class TechSpecController(MongoRepository repository, Redis redis) : Controller
{
    private readonly TechSpecRepository _repository = repository.TechSpecRepo;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var techSpecs = await redis.GetOrSetCache("techspecs", async () => await _repository.GetAll());
        if (techSpecs.Count == 0) return NotFound();

        return Ok(techSpecs);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var techSpec = await redis.GetOrSetCache($"techspec:{id}", async () => (await _repository.GetById(id))!);
        if (techSpec == null) return NotFound();

        return Ok(techSpec);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTechSpecDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newTechSpec = await _repository.Create(dto);
        if (newTechSpec == null) return NotFound();

        return Ok(newTechSpec);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldTechSpec = await _repository.Delete(id);
        if (oldTechSpec == null) return NotFound();

        return Ok(oldTechSpec);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateTechSpecDTO dto)
    {
        var discount = await _repository.GetById(id);
        if (discount == null) return NotFound();

        var updatedDiscount = await _repository.Update(id, dto);
        if (updatedDiscount == null) return NotFound();

        return Ok(updatedDiscount);
    }
}