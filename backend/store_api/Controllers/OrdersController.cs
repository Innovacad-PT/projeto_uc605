using Microsoft.AspNetCore.Mvc;
using store_api.Dtos.Orders;
using store_api.Repositories;

namespace store_api.Controllers;

[ApiController]
[Route("/orders")]
public class OrdersController : Controller
{
    private static readonly OrdersRepository _repository = new();

    [HttpPost]
    public IActionResult Create([FromBody] OrderAddDto dto)
    {
        return Ok(_repository.Add(dto.ToEntity()));
    }

    [HttpPut]
    public IActionResult Update([FromQuery] Guid id, [FromBody] OrderUpdateDto dto)
    {
        return Ok(_repository.Update(id, dto));
    }

    [HttpDelete]
    public IActionResult Delete([FromQuery] Guid id)
    {
        return Ok(_repository.Delete(id));
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        return Ok(_repository.GetAll());
    }

    [HttpGet]
    public IActionResult GetByOrderId([FromQuery] Guid id)
    {
        return Ok(_repository.GetById(id));
    }

    [HttpGet]
    public IActionResult GetByUserId([FromQuery] Guid id)
    {
        return Ok(_repository.GetOrdersByUser(id));
    }

    [HttpGet]
    public IActionResult CalculateTotal([FromQuery] Guid id)
    {
        return Ok(_repository.CalculateOrderTotal(id));
    }
}