using Microsoft.Extensions.Options;
using mongo_api.Dtos;
using MongoDB.Driver;
using mongo_api.Entities;
using System.Security.Cryptography;
using System.Text;

namespace mongo_api.Repositories;

public class MongoRepository
{
    private readonly IMongoCollection<UserEntity> _usersCollection;
    private readonly IMongoCollection<ProductEntity> _productsCollection;
    private readonly IMongoCollection<BrandEntity> _brandsCollection;
    private readonly IMongoCollection<CategoryEntity> _categoriesCollection;
    private readonly IMongoCollection<DiscountEntity> _discountsCollection;
    private readonly IMongoCollection<OrderEntity> _reviewsCollection;
    private readonly IMongoCollection<OrderEntity> _techspecsCollection;

    public MongoRepository(IOptions<MongoEntity> mongoDbSettings)
    {
        var settings = MongoClientSettings.FromUrl(
            new MongoUrl($"mongodb://{mongoDbSettings.Value.Username}:{mongoDbSettings.Value.Password}@{mongoDbSettings.Value.EndPoint}/")
        );

        var client = new MongoClient(settings);
        var database = client.GetDatabase(mongoDbSettings.Value.Database);

        _usersCollection = database.GetCollection<UserEntity>("users");
        _productsCollection = database.GetCollection<ProductEntity>("products");
        _brandsCollection = database.GetCollection<BrandEntity>("brands");
        _categoriesCollection = database.GetCollection<CategoryEntity>("categories");
        _discountsCollection = database.GetCollection<DiscountEntity>("discounts");
        _reviewsCollection = database.GetCollection<OrderEntity>("reviews");
        _techspecsCollection = database.GetCollection<OrderEntity>("techspecs");
    }

    // Users

    public async Task<List<UserEntity>> GetUsers()
    {
        var cursor = await _usersCollection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }

    public async Task<UserEntity?> GetUser(Guid id)
    {
        var cursor = await _usersCollection.FindAsync(u => u.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<UserEntity?> CreateUser(CreateUserDTO dto)
    {
        var passBytes = Encoding.UTF8.GetBytes(dto.Password);
        var passHash = SHA3_256.Create().ComputeHash(passBytes);
        dto.Password = Convert.ToHexString(passHash);

        await _usersCollection.InsertOneAsync(dto.ToEntity());

        return await GetUser(dto.Id);
    }

    /*
    public async Task<UserEntity?> UpdateUser(UserDTO dto)
    {
        var updateResult = await _usersCollection.UpdateOneAsync(
            u => u.Id.Equals(dto.Id),
            Builders<UserEntity>.Update
                .Set(u => u.Name, dto.Name)
                .Set(u => u.Password, dto.Password)
        );

        if (updateResult.MatchedCount == 0 || updateResult.ModifiedCount == 0) return null;

        return await GetUser(dto.Id);
    }*/

    public async Task<UserEntity?> DeleteUser(Guid id)
    {
        var user = await GetUser(id);
        if (user == null) return null;

        var result = await _usersCollection.DeleteOneAsync(u => u.Id.Equals(user.Id));

        return result.DeletedCount == 0 ? null : user;
    }

    // Products

    public async Task<List<ProductEntity>> GetProducts() {
        var cursor = await _productsCollection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }

    public async Task<ProductEntity?> GetProduct(Guid id)
    {
        var cursor = await _productsCollection.FindAsync(u => u.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<ProductEntity?> CreateProduct(CreateProductDTO dto)
    {
        await _productsCollection.InsertOneAsync(dto.ToEntity());

        return await GetProduct(dto.Id);
    }

    /*
    public async Task<ProductEntity?> UpdateProduct(ProductDTO dto)
    {
        var updateResult = await _productsCollection.UpdateOneAsync(
            p => p.Id.Equals(dto.Id),
            Builders<ProductEntity>.Update
                .Set(p => p.Name, dto.Name)
                .Set(p => p.Details, dto.Details)
                .Set(p => p.TechInfo, dto.TechInfo)
                .Set(p => p.Price, dto.Price)
        );
        
        if (updateResult.MatchedCount == 0 || updateResult.ModifiedCount == 0) return null;

        return await GetProduct(dto.Id);
    }*/

    public async Task<ProductEntity?> DeleteProduct(Guid id)
    {
        var product = await GetProduct(id);
        if (product == null) return null;

        var result = await _productsCollection.DeleteOneAsync(p => p.Id.Equals(product.Id));

        return result.DeletedCount == 0 ? null : product;
    }

    // Brands

    public async Task<List<BrandEntity>> GetBrands() {
        var cursor = await _brandsCollection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }

}