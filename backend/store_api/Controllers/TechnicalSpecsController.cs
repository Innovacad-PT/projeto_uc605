using Microsoft.AspNetCore.Mvc;
using store_api.Dtos;
using store_api.Dtos.TechnicalSpecs;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/technicalspecs")]
public class TechnicalSpecsController : Controller  
{
    
    private readonly TechnicalSpecsService _service = new TechnicalSpecsService();

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<IEnumerable<TechnicalSpecsEntity>?> specs = _service.GetAll();

        if (specs is Failure<IEnumerable<TechnicalSpecsEntity>?>)
        {
             return NotFound(specs as Failure<IEnumerable<TechnicalSpecsEntity>?>);
        }
        
        return Ok(specs as Success<IEnumerable<TechnicalSpecsEntity>?>);
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Result<TechnicalSpecsEntity?> spec = _service.GetById(id);

        if (spec is Failure<TechnicalSpecsEntity>)
        {
            return NotFound(spec as Failure<TechnicalSpecsEntity>);
        }
        
        return Ok(spec as Success<TechnicalSpecsEntity>);
    }

    [HttpGet("key/{key}")]
    public async Task<IActionResult> GetByKey(string key)
    {
        Result<TechnicalSpecsEntity?> spec = _service.GetByKey(key);

        if (spec is Failure<TechnicalSpecsEntity>)
        {
            return NotFound(spec as Failure<TechnicalSpecsEntity>);
        }
        
        return Ok(spec as Success<TechnicalSpecsEntity>);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] TechnicalSpecsAddDto dto)
    {
        Result<TechnicalSpecsEntity?> spec = _service.Create(dto);

        if (spec is Failure<TechnicalSpecsEntity>)
        {
            return BadRequest(spec as Failure<TechnicalSpecsEntity>);
        }
        
        return Ok(spec as Success<TechnicalSpecsEntity>);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] TechnicalSpecsUpdateDto dto)
    {
        Result<TechnicalSpecsEntity?> spec = _service.Update(id, dto);

        if (spec is Failure<TechnicalSpecsEntity>)
        {
            return BadRequest(spec as Failure<TechnicalSpecsEntity>);
        }
        
        return Ok(spec as Success<TechnicalSpecsEntity>);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        Result<TechnicalSpecsEntity?> spec  = _service.Delete(id);

        if (spec is Failure<TechnicalSpecsEntity>)
        {
            return BadRequest(spec as Failure<TechnicalSpecsEntity>);
        }
        
        return Ok(spec as Success<TechnicalSpecsEntity>);
    }
}