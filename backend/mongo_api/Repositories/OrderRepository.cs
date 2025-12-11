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
        var order = GetById(dto.Id);
        if (order != null) return null;

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

    public async Task<OrderEntity?> Update(Guid id, UpdateOrdersDTO dto)
    {
        var order = await GetById(id);
        if (order == null) return null;

        var result = await _collection.UpdateOneAsync(
            b => b.Id.Equals(order.Id),
            Builders<OrderEntity>.Update
                .Set(u => u.Status, dto.Status ?? order.Status)
        );

        if (result.MatchedCount == 0 || result.ModifiedCount == 0) return null;

        return await GetById(id);
    }
}