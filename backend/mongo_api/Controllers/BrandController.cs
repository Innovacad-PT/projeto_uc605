using Microsoft.AspNetCore.Mvc;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/brands")]
public class BrandController(MongoRepository repository) : Controller
{
    private readonly BrandRepository _repository = repository.BrandRepo;

    [HttpGet]
    public async Task<IActionResult> GetBrands()
    {
        var products = await _repository.GetBrands();
        if (products.Count == 0) return NotFound();

        return Ok(Json(products));
    }
}