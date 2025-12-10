
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController : Controller
{
    private readonly ProductsService _productsService = new();

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        Result<IEnumerable<ProductEntity>?> products = await _productsService.GetAllProducts("", "", 0, 0);

        if (products is Failure<IEnumerable<ProductEntity>?> productsFailure)
        {
            return NotFound(products);
        }

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        Result<ProductEntity?> product = _productsService.GetProductById(id);

        if (product is Failure<ProductEntity?>)
        {
            return NotFound(product);
        }
        
        return Ok(product);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto dto)
    {
        
        var res = await _productsService.CreateProduct(dto);

        if (res is Failure<ProductEntity>)
        {
            return BadRequest(res);
        }
        
        return Ok(res);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromForm] ProductUpdateDto dto)
    {
        Result<ProductEntity?> product = await _productsService.UpdateProduct(id, dto);

        if (product is Failure<ProductEntity?>)
        {
            return BadRequest(product);
        }

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        Result<ProductEntity?> product = _productsService.DeleteProduct(id);

        if (product is Failure<ProductEntity>)
        {
            return BadRequest(product);
        }

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetWithFilters([FromQuery] String search, [FromQuery] String category, [FromQuery] decimal minPice, [FromQuery] decimal maxPrice)
    {
        Result<IEnumerable<ProductEntity>?> products = await _productsService.GetAllProducts(search, category, minPice, maxPrice);

        if (products == null)
        {
            return NotFound(new Failure<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_NOT_FOUND, "Products not found."));
        }
        
        return Ok(products);
    }

    [HttpGet("{id}/discount")]
    public async Task<IActionResult> GetDiscount([FromRoute] Guid id)
    {
        Result<DiscountEntity?> discount = _productsService.GetActiveDiscount(id);

        if (discount is Failure<DiscountEntity?>)
        {
            return NotFound(discount);
        }

        return Ok(discount);
    }

    [HttpPatch("{id}/increase")]
    public IActionResult IncreaseProduct([FromRoute] Guid id, [FromBody] int increaseAmount)
    {
        Result<ProductEntity?> product = _productsService.IncreaseStock(id, increaseAmount);

        if (product is Failure<ProductEntity>)
        {
            return BadRequest(product);
        }
        
        return Ok(product);
    }
    
    [HttpPatch("{id}/decrease")]
    public IActionResult DecreaseProduct([FromRoute] Guid id, [FromBody] int increaseAmount)
    {
        Result<ProductEntity?> product = _productsService.DecreaseStock(id, increaseAmount);

        if (product is Failure<ProductEntity>)
        {
            return BadRequest(product);
        }
        
        return Ok(product);
    }

    [HttpPut("{id}/technicalSpecs")]
    public IActionResult AddTechnicalSpecs([FromRoute] Guid id, List<ProductTechnicalSpecsEntity> list)
    {
        Result<ProductEntity?> product = _productsService.AddTechnicalSpecs(id, list);

        if (product is Failure<ProductEntity?>)
        {
            return BadRequest(product);
        }

        return Ok(product);
    }
}