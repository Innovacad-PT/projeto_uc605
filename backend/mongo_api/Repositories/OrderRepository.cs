using mongo_api.Dtos.Orders;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class OrderRepository(IMongoCollection<OrderEntity> collection)
{
    private readonly IMongoCollection<OrderEntity> _collection = collection;
    
    public async Task<List<OrderEntity>> GetAll() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }
    
    public async Task<OrderEntity?> GetById(Guid id)
    {
        var cursor = await _collection.FindAsync(o => o.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }
    
    public async Task<OrderEntity?> Create(CreateOrderDTO dto)
    {
        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }
    
    public async Task<OrderEntity?> Delete(Guid id)
    {
        var order = await GetById(id);
        if (order == null) return null;

        var result = await _collection.DeleteOneAsync(o => o.Id.Equals(order.Id));

        return result.DeletedCount == 0 ? null : order;
    }
}