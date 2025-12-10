using mongo_api.Dtos;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class ProductRepository(IMongoCollection<ProductEntity> collection)
{
    private readonly IMongoCollection<ProductEntity> _collection = collection;
    
    public async Task<List<ProductEntity>> GetAll() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }

    public async Task<ProductEntity?> GetById(Guid id)
    {
        var cursor = await _collection.FindAsync(u => u.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<ProductEntity?> Create(CreateProductDTO dto)
    {
        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }

    /*
    public async Task<ProductEntity?> Update(ProductDTO dto)
    {
        var updateResult = await _productsCollection.UpdateOneAsync(
            p => p.Id.Equals(dto.Id),
            Builders<ProductEntity>.Update
                .Set(p => p.Name, dto.Name)
                .Set(p => p.Details, dto.Details)
                .Set(p => p.TechInfo, dto.TechInfo)
                .Set(p => p.Price, dto.Price)
        );

        if (updateResult.MatchedCount == 0 || updateResult.ModifiedCount == 0) return null;

        return await GetById(dto.Id);
    }*/

    public async Task<ProductEntity?> Delete(Guid id)
    {
        var product = await GetById(id);
        if (product == null) return null;

        var result = await _collection.DeleteOneAsync(p => p.Id.Equals(product.Id));

        return result.DeletedCount == 0 ? null : product;
    }
}