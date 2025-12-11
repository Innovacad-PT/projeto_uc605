using Microsoft.AspNetCore.Mvc;
using store_api.Dtos.Discounts;
using store_api.Entities;
using store_api.Repositories;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/discounts")]
public class DiscountsController : Controller
{

    private readonly DiscountService _service;

    public DiscountsController(IConfiguration configuration)
    {
        _service = new(configuration);
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        Result<IEnumerable<DiscountEntity>?> result = _service.GetAll();
        
        return Ok(result);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(Guid id)
    {
        Result<DiscountEntity?> result = _service.GetById(id);

        if (result is Failure<DiscountEntity?> failure)
        {
            return BadRequest(failure);
        }
        
        return Ok(result);
    }

    [HttpPost]
    public IActionResult Create([FromBody] DiscountAddDto dto)
    {
        Result<DiscountEntity?> result = _service.AddDiscount(dto);

        if (result is Failure<DiscountEntity?> failure)
        {
            return BadRequest(failure);
        }

        return Ok(result);
    }

    [HttpPut("{id}")]
    public IActionResult Update(Guid id, [FromBody] DiscountUpdateDto dto)
    {
        Result<DiscountEntity?> result = _service.Update(id, dto);

        if (result is Failure<DiscountEntity?> failure)
        {
            return BadRequest(failure);
        }
        
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public IActionResult Delete(Guid id)
    {
        Result<DiscountEntity?> result = _service.Delete(id);

        if (result is Failure<DiscountEntity?> failure)
        {
            return BadRequest(failure);
        }
        
        return Ok(result);
    }

}