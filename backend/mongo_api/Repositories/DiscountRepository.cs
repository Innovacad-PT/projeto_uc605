using mongo_api.Dtos.Discounts;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class DiscountRepository(IMongoCollection<DiscountEntity> collection)
{
    private readonly IMongoCollection<DiscountEntity> _collection = collection;
    
    public async Task<List<DiscountEntity>> GetAll() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }
    
    public async Task<DiscountEntity?> GetById(Guid id)
    {
        var cursor = await _collection.FindAsync(d => d.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }
    
    public async Task<DiscountEntity?> Create(CreateDiscountDTO dto)
    {
        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }
    
    public async Task<DiscountEntity?> Delete(Guid id)
    {
        var discount = await GetById(id);
        if (discount == null) return null;

        var result = await _collection.DeleteOneAsync(d => d.Id.Equals(discount.Id));

        return result.DeletedCount == 0 ? null : discount;
    }
}