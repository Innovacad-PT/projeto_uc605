using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class DiscountRepository(IMongoCollection<DiscountEntity> collection)
{
    private readonly IMongoCollection<DiscountEntity> _collection = collection;
}