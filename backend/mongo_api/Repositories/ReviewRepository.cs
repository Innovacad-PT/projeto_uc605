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
        var review = GetById(dto.Id);
        if (review != null) return null;

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

    public async Task<ReviewEntity?> Update(Guid id, UpdateReviewsDTO dto)
    {
        var review = await GetById(id);
        if (review == null) return null;

        var result = await _collection.UpdateOneAsync(
            b => b.Id.Equals(review.Id),
            Builders<ReviewEntity>.Update
                .Set(u => u.Rating, dto.Rating ?? review.Rating)
                .Set(u => u.Comment, dto.Comment ?? review.Comment)
        );

        if (result.MatchedCount == 0 || result.ModifiedCount == 0) return null;

        return await GetById(id);
    }
}