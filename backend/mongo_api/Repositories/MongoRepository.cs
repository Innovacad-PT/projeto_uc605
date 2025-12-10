using Microsoft.Extensions.Options;
using mongo_api.Dtos;
using MongoDB.Driver;
using mongo_api.Entities;
using System.Security.Cryptography;
using System.Text;

namespace mongo_api.Repositories;

public class MongoRepository
{
    public readonly BrandRepository BrandRepo;
    public readonly CategoryRepository CategoryRepo;
    public readonly DiscountRepository DiscountRepo;
    public readonly OrderRepository OrderRepo;
    public readonly ProductRepository ProductRepo;
    public readonly ReviewRepository ReviewRepo;
    public readonly TechSpecRepository TechSpecRepo;
    public readonly UserRepository UserRepo;
    
    
    public MongoRepository(IOptions<MongoEntity> mongoDbSettings)
    {
        var settings = MongoClientSettings.FromUrl(
            new MongoUrl($"mongodb://{mongoDbSettings.Value.Username}:{mongoDbSettings.Value.Password}@{mongoDbSettings.Value.EndPoint}/")
        );

        var client = new MongoClient(settings);
        var database = client.GetDatabase(mongoDbSettings.Value.Database);
        
        BrandRepo = new BrandRepository(database.GetCollection<BrandEntity>("brands"));
        CategoryRepo = new CategoryRepository(database.GetCollection<CategoryEntity>("categories"));
        DiscountRepo = new DiscountRepository(database.GetCollection<DiscountEntity>("discounts"));
        OrderRepo = new OrderRepository(database.GetCollection<OrderEntity>("orders"));
        ProductRepo = new ProductRepository(database.GetCollection<ProductEntity>("products"));
        ReviewRepo = new ReviewRepository(database.GetCollection<ReviewEntity>("reviews"));
        TechSpecRepo = new TechSpecRepository(database.GetCollection<TechnicalSpecEntity>("techspecs"));
        UserRepo = new UserRepository(database.GetCollection<UserEntity>("users"));
    }

}