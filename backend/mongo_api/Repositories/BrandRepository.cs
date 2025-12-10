using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class BrandRepository(IMongoCollection<BrandEntity> collection)
{
    private readonly IMongoCollection<BrandEntity> _collection = collection;
    
    public async Task<List<BrandEntity>> GetBrands() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }
}