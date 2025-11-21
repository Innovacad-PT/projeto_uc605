using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;
using Swashbuckle.AspNetCore.Annotations;

namespace store_api.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController : Controller
{
    private readonly ProductsService _productsService = new ProductsService();

    [HttpGet("")]
    public async Task<IActionResult> GetAll()
    {
        return Ok(await _productsService.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] Guid id)
    {
        var res = await _productsService.GetById(id);

        if (res.HasError || res is Failure<ProductEntity>)
        {
            var failure = (Failure<ProductEntity>)res;

            if (res.GetCode() == ResultCode.USER_NOT_FOUND)
                return NotFound(failure);
            
            return BadRequest(failure);
        }
        
        var success = (Success<ProductEntity>)res;

        return Ok(success);
    }

    [Authorize]
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto product)
    {
        var res = await _productsService.CreateProduct(product);

        if (res.HasError || res is Failure<ProductEntity>)
        {
            var failure = (Failure<ProductEntity>)res;
            return BadRequest(failure);
        }
        
        return Ok(res);
    }

    [Authorize]
    [HttpPut]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProduct([FromQuery] Guid id, [FromForm] ProductUpdateDto product)
    {
        var res = await _productsService.UpdateProduct(id, product);

        if (res.HasError || res is Failure<ProductEntity>)
        {
            var failure = (Failure<ProductEntity>)res;
            return BadRequest(failure);
        }
        
        return Ok(res);
    }

    [Authorize]
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct(String id)
    {
        var res = await _productsService.DeleteProduct(id);

        if (res.HasError)
        {
            var failure = (Failure<ProductEntity>)res;

            if(res.GetCode() == ResultCode.PRODUCT_NOT_FOUND)
                return NotFound(failure);

            return BadRequest(failure);
        }

        var success = (Success<ProductEntity>)res;
        return Ok(success);
    }
}