using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class TechSpecRepository(IMongoCollection<TechnicalSpecEntity> collection)
{
    private readonly IMongoCollection<TechnicalSpecEntity> _collection = collection;
}