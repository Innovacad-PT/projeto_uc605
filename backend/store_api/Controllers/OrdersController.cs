using Microsoft.AspNetCore.Mvc;
using store_api.Dtos.Orders;
using store_api.Entities;
using store_api.Repositories;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;


[ApiController]
[Route("/orders")]
public class OrdersController(IConfiguration configuration) : Controller
{
    private readonly OrdersService _service = new(configuration);

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderAddDto dto)
    {
        Result<OrderEntity?> result = await _service.CreateOrder(dto);

        if (result is Failure<OrderEntity> failure)
            return BadRequest(failure);
        
        return Ok(result as Success<OrderEntity>);
    }

    [HttpPut]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] OrderUpdateDto<OrderEntity> dto)
    {
        Result<OrderEntity?> result = await _service.Update(id, dto);

        if (result is Failure<OrderEntity> failure)
            return BadRequest(failure);

        return Ok(result as Success<OrderEntity>);
    }

    [HttpDelete]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        Result<OrderEntity?> result = await _service.Delete(id);

        if (result is Failure<OrderEntity> failure)
            return BadRequest(failure);

        return Ok(result as Success<OrderEntity>);
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        Result<IEnumerable<OrderEntity>>? result = await _service.GetAllOrder();

        if (result is Failure<IEnumerable<OrderEntity>> failure)
            return BadRequest(failure);

        return Ok(result as Success<IEnumerable<OrderEntity>>);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetByOrderId([FromRoute] Guid id)
    {
        Result<OrderEntity?> result = await _service.GetOrderById(id);

        if (result is Failure<OrderEntity> failure)
            return BadRequest(failure);

        return Ok(result as Success<OrderEntity>);
    }

    [HttpGet("user/{id}")]
    public async Task<IActionResult> GetByUserId([FromRoute] Guid id)
    {
        Result<IEnumerable<OrderEntity>?> result = await _service.GetOrdersByUser(id);

        if (result is Failure<IEnumerable<OrderEntity>> failure)
            return BadRequest(failure);

        return Ok(result as Success<IEnumerable<OrderEntity>>);
    }

    [HttpGet("{id}/total")]
    public async Task<IActionResult> CalculateTotal([FromRoute] Guid id)
    {
        var result = await _service.CalculateOrderTotal(id);

        if (result is Failure<decimal> failure)
            return BadRequest(failure);

        return Ok(result as Success<decimal>);
    }
}