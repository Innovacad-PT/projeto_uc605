using Microsoft.AspNetCore.Mvc;
using store_api.Dtos.Orders;
using store_api.Entities;
using store_api.Repositories;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;


[ApiController]
[Route("/orders")]
public class OrdersController : Controller
{
    private static readonly OrdersService _service = new();

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] OrderAddDto dto)
    {
        Result<OrderEntity?> result = await _service.CreateOrder(dto);

        if (result is Failure<OrderEntity> failure)
        {
            return BadRequest(failure);
        }
        
        return Ok(result as Success<OrderEntity>);
    }

    [HttpPut]
    public IActionResult Update([FromRoute] int id, [FromBody] OrderUpdateDto dto)
    {
        return Ok(_service.Update(id, dto));
    }

    [HttpDelete]
    public IActionResult Delete([FromRoute] int id)
    {
        return Ok(_service.Delete(id));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_service.GetAllOrder());
    }

    [HttpGet("{id}")]
    public IActionResult GetByOrderId([FromRoute] int id)
    {
        return Ok(_service.GetOrderById(id));
    }

    [HttpGet("user/{id}")]
    public IActionResult GetByUserId([FromRoute] Guid id)
    {
        return Ok(_service.GetOrdersByUser(id));
    }

    [HttpGet("{id}/total")]
    public IActionResult CalculateTotal([FromRoute] int id)
    {
        return Ok(_service.CalculateOrderTotal(id));
    }
}