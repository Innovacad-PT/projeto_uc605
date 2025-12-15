using Microsoft.AspNetCore.Mvc;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController(IConfiguration configuration) : Controller
{
    private readonly ProductsService _productsService = new (configuration);

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        Result<IEnumerable<ProductEntity>?> products = await _productsService.GetAllProducts("", "", 0, 0);

        if (products is Failure<IEnumerable<ProductEntity>?> failure)
            return NotFound(products);

        return Ok(products);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById([FromRoute] Guid id)
    {
        Result<ProductEntity?> product = await _productsService.GetProductById(id);

        if (product is Failure<ProductEntity?> failure)
            return NotFound(failure);
        
        return Ok(product);
    }

    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> CreateProduct([FromForm] ProductCreateDto dto)
    {
        Result<ProductEntity?> product = await _productsService.CreateProduct(dto);

        if (product is Failure<ProductEntity> failure)
            return BadRequest(failure);
        
        return Ok(product);
    }

    [HttpPut("{id}")]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UpdateProduct([FromRoute] Guid id, [FromForm] ProductUpdateDto<ProductEntity> dto)
    {
        Result<ProductEntity?> product = await _productsService.UpdateProduct(id, dto);

        if (product is Failure<ProductEntity?> failure)
            return BadRequest(failure);

        return Ok(product);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteProduct([FromRoute] Guid id)
    {
        Result<ProductEntity?> product = await _productsService.DeleteProduct(id);

        if (product is Failure<ProductEntity> failure)
            return BadRequest(failure);

        return Ok(product);
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetWithFilters([FromQuery] String search, [FromQuery] String category, [FromQuery] decimal minPice, [FromQuery] decimal maxPrice)
    {
        Result<IEnumerable<ProductEntity>?> products = await _productsService.GetAllProducts(search, category, minPice, maxPrice);

        if (products == null)
            return NotFound(
                new Failure<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_NOT_FOUND,
                    "Products not found.")
            );

        return Ok(products);
    }

    [HttpGet("{id}/discount")]
    public async Task<IActionResult> GetDiscount([FromRoute] Guid id)
    {
        Result<DiscountEntity?> discount = await _productsService.GetActiveDiscount(id);

        if (discount is Failure<DiscountEntity?> failure)
        {
            return NotFound(failure);
        }

        return Ok(discount);
    }

    [HttpPatch("{id}/increase")]
    public async Task<IActionResult> IncreaseProduct([FromRoute] Guid id, [FromBody] int increaseAmount)
    {
        Result<ProductEntity?> product = await _productsService.IncreaseStock(id, increaseAmount);

        if (product is Failure<ProductEntity> failure)
            return BadRequest(failure);
        
        return Ok(product);
    }
    
    [HttpPatch("{id}/decrease")]
    public async Task<IActionResult> DecreaseProduct([FromRoute] Guid id, [FromBody] int increaseAmount)
    {
        Result<ProductEntity?> product = await _productsService.DecreaseStock(id, increaseAmount);

        if (product is Failure<ProductEntity> failure)
            return BadRequest(failure);
        
        return Ok(product);
    }

    [HttpPut("{id}/technicalSpecs")]
    public async Task<IActionResult> AddTechnicalSpecs([FromRoute] Guid id, List<Guid> list)
    {
        Result<ProductEntity?> product = await _productsService.AddTechnicalSpecs(id, list);

        if (product is Failure<ProductEntity?> failure)
            return BadRequest(failure);

        return Ok(product);
    }

    [HttpDelete("{id}/technicalSpecs")]
    public async Task<IActionResult> RemoveTechnicalSpecs([FromRoute] Guid id, [FromBody] Guid specId)
    {
        Result<ProductEntity?> product = await _productsService.RemoveTechnicalSpec(id, specId);

        if (product is Failure<ProductEntity?> failure)
            return BadRequest(failure);

        return Ok(product);
    }
}