using Microsoft.AspNetCore.Mvc;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/discounts")]
public class DiscountController(MongoRepository repository) : Controller
{
    private readonly DiscountRepository _repository = repository.DiscountRepo;
}