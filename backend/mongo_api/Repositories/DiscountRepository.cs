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
        var cursor = await _collection.FindAsync(d => d.Id.Equals(id) || d.ProductId.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }
    
    public async Task<DiscountEntity?> Create(CreateDiscountDTO dto)
    {
        var discounts = await GetAll();

        foreach (var discount in discounts)
        {
           if (discount.Id == dto.Id) return null;
           if (discount.ProductId == dto.ProductId) return null;
        }
        
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

    public async Task<DiscountEntity?> Update(Guid id, UpdateDiscountDTO dto)
    {
        var discount = await GetById(id);
        if (discount == null) return null;

        var result = await _collection.UpdateOneAsync(
            b => b.Id.Equals(discount.Id),
            Builders<DiscountEntity>.Update
                .Set(u => u.ProductId, dto.ProductId ?? discount.ProductId)
                .Set(u => u.Percentage, dto.Percentage ?? discount.Percentage)
                .Set(u => u.StartDate, dto.StartDate ?? discount.StartDate)
                .Set(u => u.EndDate, dto.EndDate ?? discount.EndDate)
        );

        if (result.MatchedCount == 0 || result.ModifiedCount == 0) return null;

        return await GetById(id);
    }
}