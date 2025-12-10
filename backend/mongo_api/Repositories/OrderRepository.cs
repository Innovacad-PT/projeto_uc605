using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class OrderRepository(IMongoCollection<OrderEntity> collection)
{
    private readonly IMongoCollection<OrderEntity> _collection = collection;
}