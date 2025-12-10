using mongo_api.Dtos.Category;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class CategoryRepository(IMongoCollection<CategoryEntity> collection)
{
    private readonly IMongoCollection<CategoryEntity> _collection = collection;
    
    public async Task<List<CategoryEntity>> GetAll() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }
    
    public async Task<CategoryEntity?> GetById(Guid id)
    {
        var cursor = await _collection.FindAsync(c => c.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }
    
    public async Task<CategoryEntity?> Create(CreateCategoryDTO dto)
    {
        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }
    
    public async Task<CategoryEntity?> Delete(Guid id)
    {
        var category = await GetById(id);
        if (category == null) return null;

        var result = await _collection.DeleteOneAsync(b => b.Id.Equals(category.Id));

        return result.DeletedCount == 0 ? null : category;
    }
}