using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using store_api.Dtos;
using store_api.Dtos.Users;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("users")]
public class UsersController(IConfiguration configuration) : Controller
{
    private readonly UsersService _service = new(configuration);

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        Result<IEnumerable<UserEntity>?> result = await _service.GetAll();

        if (result is Failure<IEnumerable<UserEntity>?> failure)
            return BadRequest(failure);

        return Ok(result as  Success<IEnumerable<UserEntity>>);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var res = await _service.GetById(id);

        if (res.HasError)
        {
            var failure = (Failure<UserEntity>)res;
            return BadRequest(failure);
        }

        var success = (Success<UserEntity>)res;
        return Ok(success);
    }

    [HttpPost("/register")]
    public async Task<IActionResult> Create([FromBody] UserRegisterDto dto)
    {
        Result<UserEntity?> result = await _service.Create(dto);

        if (result is Failure<UserEntity?> failure)
            return BadRequest(failure);

        return Ok(result as  Success<UserEntity>);
    }

    [HttpPost("/login")]
    public async Task<IActionResult> LoginUser([FromBody] UserLoginDto<UserEntity> login)
    {
        Console.WriteLine(login.Password);
        Result<UserLoggedInDao?> result = await _service.Login(login);

        if (result is Failure<UserLoggedInDao?> failure)
            return BadRequest(failure);
        
        return Ok(result  as Success<UserLoggedInDao>);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete([FromRoute] Guid id)
    {
        Result<UserEntity?> result = await _service.Delete(id);

        if (result is Failure<UserEntity?> failure)
            return BadRequest(failure);

        return Ok(result as Success<UserEntity>);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UserUpdateDto<UserEntity> dto)
    {
        Result<UserEntity?> result = await _service.Update(id, dto);

        if (result is Failure<UserEntity?> failure)
            return BadRequest(failure);
        
        return Ok(result as Success<UserEntity>);
    }
}