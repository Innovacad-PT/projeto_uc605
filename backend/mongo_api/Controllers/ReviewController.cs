using Microsoft.AspNetCore.Mvc;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/reviews")]
public class ProductCategory(MongoRepository repository) : Controller
{

}