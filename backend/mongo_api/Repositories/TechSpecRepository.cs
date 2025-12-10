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
}