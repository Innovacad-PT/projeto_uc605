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
    
}