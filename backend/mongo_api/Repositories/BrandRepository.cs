using mongo_api.Dtos.Brands;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class BrandRepository(IMongoCollection<BrandEntity> collection)
{
    private readonly IMongoCollection<BrandEntity> _collection = collection;
    
    public async Task<List<BrandEntity>> GetAll() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }
    
    public async Task<BrandEntity?> GetById(Guid id)
    {
        var cursor = await _collection.FindAsync(b => b.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }
    
    public async Task<BrandEntity?> Create(CreateBrandDTO dto)
    {
        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }
    
    public async Task<BrandEntity?> Delete(Guid id)
    {
        var brand = await GetById(id);
        if (brand == null) return null;

        var result = await _collection.DeleteOneAsync(b => b.Id.Equals(brand.Id));

        return result.DeletedCount == 0 ? null : brand;
    }
}