using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using store_api.Dtos.Categories;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/categories")]
public class CategoriesController : ControllerBase
{
    
    private readonly CategoriesService _service = new();

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<IEnumerable<CategoryEntity>> _categories = _service.GetAllCategories();

        if (_categories is Failure<IEnumerable<CategoryEntity>?> categories)
        {
            return NotFound(categories);
        }
        
        return Ok(_categories);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CategoryAddDto dto)
    {
        Result<CategoryEntity> result = _service.CreateCategory(dto);

        if (result is Failure<CategoryEntity?> category)
        {
            return BadRequest(category);
        }
        
        return Ok(result);
    }
    
}