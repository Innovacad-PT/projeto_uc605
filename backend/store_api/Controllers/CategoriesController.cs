using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using store_api.Dtos.Categories;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/categories")]
public class CategoriesController(IConfiguration configuration) : ControllerBase
{
    
    private readonly CategoriesService _service = new(configuration);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<IEnumerable<CategoryEntity>?> categories = await _service.GetAllCategories();
        if (categories is Failure<IEnumerable<CategoryEntity>?> failure) return NotFound(failure);
        
        return Ok(categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryAddDto dto)
    {
        Result<CategoryEntity?> category = await _service.CreateCategory(dto);
        if (category is Failure<CategoryEntity?> failure) return BadRequest(failure);

        return Ok(category);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] CategoryUpdateDto<CategoryEntity> dto)
    {
        Result<CategoryEntity?> brand = await _service.Update(id, dto);

        if (brand is Failure<CategoryEntity>)
            return BadRequest(brand as Failure<CategoryEntity>);

        return Ok(brand as Success<CategoryEntity>);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        Result<CategoryEntity?> brand  = await _service.Delete(id);

        if (brand is Failure<CategoryEntity>)
            return BadRequest(brand as Failure<CategoryEntity>);

        return Ok(brand as Success<CategoryEntity>);
    }
}