using Microsoft.AspNetCore.Mvc;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/orders")]
public class OrderCategory(MongoRepository repository) : Controller
{
    private readonly OrderRepository _repository = repository.OrderRepo;
}