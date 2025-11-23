
using Microsoft.AspNetCore.Mvc;
using store_api.Entities;
using store_api.Services;
using store_api.Utils;

namespace store_api.Controllers;

[ApiController]
[Route("/products")]
public class ProductsController : Controller
{
    private readonly ProductsService _productsService = new ProductsService();
    private readonly CategoriesService _categoriesService = new CategoriesService();

    [HttpGet]
    public async Task<IActionResult> GetAllProducts()
    {
        IEnumerable<ProductEntity>? products = await _productsService.GetAllProducts("", "", 0, 0);

        if (products == null)
        {
            return NotFound(
                new Failure<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_NOT_FOUND, "Products not found"));
        }

        return Ok(new Success<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_FOUND, "Products found", products));
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(Guid id)
    {
        ProductEntity? product = _productsService.GetProductById(id);

        if (product == null)
        {
            return NotFound(new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND, "Product not found"));
        }
        
        return Ok(new Success<ProductEntity?>(ResultCode.PRODUCT_FOUND, "Product found", product));
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct(ProductCreateDto dto)
    {
        ProductEntity? created = _productsService.CreateProduct(dto);

        if (created == null)
        {
            return BadRequest(new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_CREATED, "Product not created"));
        }
        
        return Ok(new Success<ProductEntity?>(ResultCode.PRODUCT_CREATED, "Product created", created));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(Guid id, ProductUpdateDto dto)
    {
        ProductEntity? product = _productsService.UpdateProduct(id, dto);

        if (product == null)
        {
            return BadRequest(new Failure<ProductEntity?>(ResultCode.PRODUCT_NOT_FOUND, "Product not found"));
        }

        return Ok(new Success<ProductEntity>(ResultCode.PRODUCT_UPDATED, "Product updated", product));
    }

    [HttpGet("search")]
    public async Task<IActionResult> GetWithFilters([FromQuery] String search, [FromQuery] String category, [FromQuery] decimal minPice, [FromQuery] decimal maxPrice)
    {
        IEnumerable<ProductEntity>? products = await _productsService.GetAllProducts(search, category, minPice, maxPrice);

        if (products == null)
        {
            return NotFound(new Failure<IEnumerable<ProductEntity>?>(ResultCode.PRODUCT_NOT_FOUND, "Products not found."));
        }
        
        return Ok(new Success<IEnumerable<ProductEntity>>(ResultCode.PRODUCT_FOUND,"Products found.",products));
    }

    [HttpGet("{id}/discount")]
    public async Task<IActionResult> GetDiscount([FromRoute] Guid id)
    {
        ProductEntity? discount = _productsService.ApplyDiscount(id);

        if (discount == null)
        {
            return NotFound();
        }

        return Ok(new Success<ProductEntity>(ResultCode.PRODUCT_DISCOUNT_APPLIED, "Discount applied to product.", discount));
    }


    /*[HttpGet("")]
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
    }*/
}