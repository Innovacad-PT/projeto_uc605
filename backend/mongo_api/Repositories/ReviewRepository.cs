using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class ReviewRepository(IMongoCollection<ReviewEntity> collection)
{
    private readonly IMongoCollection<ReviewEntity> _collection = collection;
}