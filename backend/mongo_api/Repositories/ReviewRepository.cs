using mongo_api.Dtos.Reviews;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class ReviewRepository(IMongoCollection<ReviewEntity> collection)
{
    private readonly IMongoCollection<ReviewEntity> _collection = collection;
    
    public async Task<List<ReviewEntity>> GetAll() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }
    
    public async Task<ReviewEntity?> GetById(Guid id)
    {
        var cursor = await _collection.FindAsync(r => r.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }
    
    public async Task<ReviewEntity?> Create(CreateReviewDTO dto)
    {
        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }
    
    public async Task<ReviewEntity?> Delete(Guid id)
    {
        var review = await GetById(id);
        if (review == null) return null;

        var result = await _collection.DeleteOneAsync(r => r.Id.Equals(review.Id));

        return result.DeletedCount == 0 ? null : review;
    }
}