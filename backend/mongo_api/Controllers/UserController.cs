using Microsoft.AspNetCore.Mvc;
using mongo_api.Dtos;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/users")]
public class UserController(MongoRepository repository) : Controller
{
    private readonly UserRepository _repository = repository.UserRepo;

    [HttpGet]
    public async Task<IActionResult> GetUsers()
    {
        var users = await _repository.GetUsers();

        if (users.Count == 0) return NotFound();

        return Ok(Json(users));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser([FromRoute] Guid id)
    {
        if (id == Guid.Empty) return NotFound();

        var user = await _repository.GetUser(id);
        if (user == null) return NotFound();

        return Ok(Json(user));
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserDTO dto)
    {
        dto.Id = Guid.NewGuid();

        var newUser = await _repository.CreateUser(dto);
        if (newUser == null) return NotFound();

        return Ok(Json(newUser));
    }
/*
    [HttpPut]
    public async Task<IActionResult> UpdateUser([FromBody] UserDTO dto)
    {
        var updatedUser = await _repository.UpdateUser(dto);
        if (updatedUser == null) return NotFound();

        return Ok(Json(updatedUser));
    }
*/
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser([FromRoute] Guid id)
    {
        var oldUser = await _repository.DeleteUser(id);
        if (oldUser == null) return NotFound();

        return Ok(Json(oldUser));
    }
}