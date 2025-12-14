using Microsoft.AspNetCore.Mvc;
using store_api.Dtos.Discounts;
using store_api.Entities;
using store_api.Repositories;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/discounts")]
public class DiscountsController(IConfiguration configuration) : Controller
{

    private readonly DiscountService _service = new(configuration);

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<IEnumerable<DiscountEntity>?> result = await _service.GetAll();
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        Result<DiscountEntity?> result = await _service.GetById(id);

        if (result is Failure<DiscountEntity?> failure)
            return BadRequest(failure);

        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] DiscountAddDto<DiscountEntity> dto)
    {
        Result<DiscountEntity?> result = await _service.AddDiscount(dto);

        if (result is Failure<DiscountEntity?> failure)
            return BadRequest(failure);

        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] DiscountUpdateDto<DiscountEntity> dto)
    {
        Result<DiscountEntity?> result = await _service.Update(id, dto);

        if (result is Failure<DiscountEntity?> failure)
            return BadRequest(failure);

        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        Result<DiscountEntity?> result = await _service.Delete(id);

        if (result is Failure<DiscountEntity?> failure)
            return BadRequest(failure);
        
        return Ok(result);
    }
}