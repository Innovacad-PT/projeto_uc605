using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos.Orders;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/orders")]
public class OrderController(MongoRepository repository) : Controller
{
    private readonly OrderRepository _repository = repository.OrderRepo;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await _repository.GetAll();
        if (orders.Count == 0) return NotFound();

        return Ok(Json(orders));
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var order = await _repository.GetById(id);
        if (order == null) return NotFound();

        return Ok(Json(order));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newOrder = await _repository.Create(dto);
        if (newOrder == null) return NotFound();

        return Ok(Json(newOrder));
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldOrder = await _repository.Delete(id);
        if (oldOrder == null) return NotFound();

        return Ok(Json(oldOrder));
    }
}