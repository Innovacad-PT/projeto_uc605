using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos.Orders;
using mongo_api.Repositories;
using mongo_api.Utils;

namespace mongo_api.Controllers;

[ApiController]
[Route("/orders")]
public class OrderController(MongoRepository repository, Redis redis) : Controller
{
    private readonly OrderRepository _repository = repository.OrderRepo;
    
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var orders = await redis.GetOrSetCache("orders", async () => await _repository.GetAll());
        if (orders == null || orders.Count == 0) return NotFound();

        return Ok(orders);
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var order = await redis.GetOrSetCache($"order:{id}", async () => (await _repository.GetById(id))!);
        if (order == null) return NotFound();

        return Ok(order);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateOrderDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newOrder = await _repository.Create(dto);
        if (newOrder == null) return NotFound();

        return Ok(newOrder);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldOrder = await _repository.Delete(id);
        if (oldOrder == null) return NotFound();

        return Ok(oldOrder);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateOrdersDTO dto)
    {
        var order = await _repository.GetById(id);
        if (order == null) return NotFound();

        var updatedOrder = await _repository.Update(id, dto);
        if (updatedOrder == null) return NotFound();

        return Ok(updatedOrder);
    }
}