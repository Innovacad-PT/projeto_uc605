using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Newtonsoft.Json;
using store_api.Dtos;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("users")]
public class UsersController : Controller
{
    /*private UsersService _service = new UsersService();
    static List<UserEntity> users = new List<UserEntity>([new(Guid.NewGuid(), "Pedro", "Guerra", "pedroga", "pedroga.personal@gmail.com")]);

    private IConfiguration _configuration;
    
    public UsersController(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    
    [HttpGet("")]
    public IActionResult GetAll() => Ok(Json(users));

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var res = await _service.GetById(id);

        if (res.HasError)
        {
            var failure = (Failure<UserEntity>) res;
            return BadRequest(Json(failure.GetMessage()));
        }

        var success = (Success<UserEntity>) res;
        return Ok(Json(success.GetValue()));
    }

    [HttpPost("")]
    public async Task<IActionResult> LoginUser([FromBody] LoginDto login)
    {
        if (!login.IsValid())
        {
            string json = JsonConvert.SerializeObject(new { error = "The type is not of 'username' or 'email'."});
            return BadRequest(Json(json));
        }

        var claims = new[]
        {
            new Claim(ClaimTypes.Name, login.User),
            new Claim(ClaimTypes.Role, "guest")
        };

        
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JwtSecretKey"] ?? ""));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: DateTime.Now.AddHours(1), signingCredentials: creds);

        return Ok(Json(new
        {
            token= new JwtSecurityTokenHandler().WriteToken(token),
            message= "Login efetuado com sucesso!"
        }));
    }*/
}