using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_api.Dtos.Brands;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/brands")]
public class BrandsController(IConfiguration configuration) : Controller
{
    private readonly BrandsService _service =  new(configuration);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<IEnumerable<BrandEntity>?> brands = await _service.GetAllBrands();

        return Ok(brands);
    }

    [HttpGet("id/{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Result<BrandEntity?> brand = await _service.GetById(id);

        if (brand is Failure<BrandEntity>)
            return NotFound(brand as Failure<BrandEntity>);
        
        return Ok(brand as Success<BrandEntity>);
    }

    [HttpPost]
    public async Task<IActionResult> Add([FromBody] BrandAddDto<BrandEntity> dto)
    {
        Result<BrandEntity?> brand = await _service.CreateBrand(dto);

        if (brand is Failure<BrandEntity>)
            return BadRequest(brand as Failure<BrandEntity>);
        
        return Ok(brand as Success<BrandEntity>);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromQuery] Guid id, [FromBody] BrandUpdateDto<BrandEntity> dto)
    {
        Result<BrandEntity?> brand = await _service.Update(id, dto);

        if (brand is Failure<BrandEntity>)
            return BadRequest(brand as Failure<BrandEntity>);
        
        return Ok(brand as Success<BrandEntity>);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete(Guid id)
    {
        Result<BrandEntity?> brand  = await _service.Delete(id);

        if (brand is Failure<BrandEntity>)
            return BadRequest(brand as Failure<BrandEntity>);
        
        return Ok(brand as Success<BrandEntity>);
    }
}