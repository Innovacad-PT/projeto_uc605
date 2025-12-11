using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos;
using mongo_api.Repositories;
using mongo_api.Utils;

namespace mongo_api.Controllers;

[ApiController]
[Route("/users")]
public class UserController(MongoRepository repository, Redis redis) : Controller
{
    private readonly UserRepository _repository = repository.UserRepo;

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await redis.GetOrSetCache("users", async () => await _repository.GetAll());

        if (users.Count == 0) return NotFound();

        return Ok(Json(users));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var user = await redis.GetOrSetCache($"user:{id}", async () => (await _repository.GetById(id))!);
        if (user == null) return NotFound();

        return Ok(Json(user));
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUserDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newUser = await _repository.Create(dto);
        if (newUser == null) return NotFound();

        return Ok(Json(newUser));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        var oldUser = await _repository.Delete(id);
        if (oldUser == null) return NotFound();

        return Ok(Json(oldUser));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateUserDTO dto)
    {
        var user = await _repository.GetById(id);
        if (user == null) return NotFound();

        var updatedUser = await _repository.Update(id, dto);
        if (updatedUser == null) return NotFound();

        return Ok(Json(updatedUser));
    }
}