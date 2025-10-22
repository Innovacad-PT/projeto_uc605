using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;
using StackExchange.Redis;

namespace redis_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController : Controller {
        private readonly IDatabase _redis;
        
        public RedisController(IConnectionMultiplexer redis) {
            _redis = redis.GetDatabase();
        }

        [HttpGet("get/{key}")]
        [SwaggerOperation(
            summary: "Gets a value by key",
            description: "Retrieves the value associated with the specified key from Redis."
        )]
        public IActionResult Get(string key) {
            var value = _redis.StringGet(key);

            if (value.IsNull) return NotFound(new {message = $"The key '{key}' wasn't found!"});

            return Ok(value.ToString());
        }

        [HttpPost("set/{key}/{value}")]
        [SwaggerOperation(
            summary: "Sets a value by key",
            description: "Retrieves the value associated with the specified key from Redis."
        )]
        public IActionResult Set(string key, string value) {
            if (_redis.StringSet(key, value, TimeSpan.FromMinutes(1))) return Ok();

            return StatusCode(500, new {message = $"Something went wrong while trying to set the key '{key}' with the value '{value}'"});
        }
    }
}
