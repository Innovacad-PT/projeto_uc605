using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class CategoryRepository(IMongoCollection<CategoryEntity> collection)
{
    private readonly IMongoCollection<CategoryEntity> _collection = collection;
}