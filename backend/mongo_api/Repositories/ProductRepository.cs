using mongo_api.Dtos;
using mongo_api.Entities;
using MongoDB.Driver;

namespace mongo_api.Repositories;

public class ProductRepository(IMongoCollection<ProductEntity> collection)
{
    private readonly IMongoCollection<ProductEntity> _collection = collection;
    
    public async Task<List<ProductEntity>> GetProducts() {
        var cursor = await _collection.FindAsync(_ => true);

        return await cursor.ToListAsync();
    }

    public async Task<ProductEntity?> GetProduct(Guid id)
    {
        var cursor = await _collection.FindAsync(u => u.Id.Equals(id));

        return await cursor.FirstOrDefaultAsync();
    }

    public async Task<ProductEntity?> CreateProduct(CreateProductDTO dto)
    {
        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetProduct(dto.Id);
    }

    /*
    public async Task<ProductEntity?> UpdateProduct(ProductDTO dto)
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

        return await GetProduct(dto.Id);
    }*/

    public async Task<ProductEntity?> DeleteProduct(Guid id)
    {
        var product = await GetProduct(id);
        if (product == null) return null;

        var result = await _collection.DeleteOneAsync(p => p.Id.Equals(product.Id));

        return result.DeletedCount == 0 ? null : product;
    }
}