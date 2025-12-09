using Microsoft.AspNetCore.Mvc;
using mongo_api.Repositories;

namespace mongo_api.Controllers;

[ApiController]
[Route("/techspecs")]
public class TechSepcController(MongoRepository repository) : Controller
{

}