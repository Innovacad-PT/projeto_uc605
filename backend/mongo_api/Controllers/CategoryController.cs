using Microsoft.AspNetCore.Mvc;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/categories")]
public class CategoryController(MongoRepository repository) : Controller
{

}