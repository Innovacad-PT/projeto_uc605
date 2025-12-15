using mongo_api.Dtos.TechnicalSpecs;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class TechSpecRepository(IMongoCollection<TechnicalSpecEntity> collection)
{
    private readonly IMongoCollection<TechnicalSpecEntity> _collection = collection;
    
    public async Task<List<TechnicalSpecEntity>> GetAll() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }
    
    public async Task<TechnicalSpecEntity?> GetById(Guid id)
    {
        var cursor = await _collection.FindAsync(t => t.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<TechnicalSpecEntity?> Create(CreateTechSpecDTO dto)
    {

        if (string.IsNullOrWhiteSpace(dto.Key))
        {
            return null;
        }

        TechnicalSpecEntity? spec = await GetById(dto.Id);
        if (spec != null)
        {
            return null;
        }

        spec = _collection.Find(t => t.Key == dto.Key).FirstOrDefault();
        if (spec != null)
        {
            return null;
        }

        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }
    
    public async Task<TechnicalSpecEntity?> Delete(Guid id)
    {
        var techSpec = await GetById(id);
        if (techSpec == null) return null;

        var result = await _collection.DeleteOneAsync(t => t.Id.Equals(techSpec.Id));

        return result.DeletedCount == 0 ? null : techSpec;
    }

    public async Task<TechnicalSpecEntity?> Update(Guid id, UpdateTechSpecDTO dto)
    {
        var techSpec = await GetById(id);
        if (techSpec == null) return null;

        var result = await _collection.UpdateOneAsync(
            t => t.Id.Equals(techSpec.Id),
            Builders<TechnicalSpecEntity>.Update
                .Set(u => u.Key, dto.Key ?? techSpec.Key)
                .Set(u => u.Value, dto.Value ?? techSpec.Value)
        );

        if (result.MatchedCount == 0 || result.ModifiedCount == 0) return null;

        return await GetById(id);
    }
}