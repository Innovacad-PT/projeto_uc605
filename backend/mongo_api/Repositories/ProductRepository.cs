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
        var product = await GetById(dto.Id);
        if (product != null) return null;

        await _collection.InsertOneAsync(dto.ToEntity());

        return await GetById(dto.Id);
    }


    public async Task<ProductEntity?> Update(Guid id, UpdateProductsDTO dto)
    {
        var product = await GetById(id);
        if (product == null) return null;

        var updateResult = await _collection.UpdateOneAsync(
            p => p.Id.Equals(product.Id),
            Builders<ProductEntity>.Update
                .Set(p => p.Name, dto.Name ?? product.Name)
                .Set(p => p.Description, dto.Description ?? product.Description)
                .Set(p => p.TechnicalSpecs, dto.Specs ?? product.TechnicalSpecs)
                .Set(p => p.Price, dto.Price ?? product.Price)
                .Set(p => p.CategoryId, dto.CategoryId ?? product.CategoryId)
                .Set(p => p.BrandId, dto.BrandId ?? product.BrandId)
                .Set(p => p.Stock, dto.Stock ?? product.Stock)
                .Set(p => p.ImageUrl, product.ImageUrl ?? product.ImageUrl)
                .Set(p => p.Reviews, dto.Reviews ?? product.Reviews)
                .Set(p => p.UpdatedAt, DateTime.UtcNow)
        );

        if (updateResult.MatchedCount == 0 || updateResult.ModifiedCount == 0) return null;

        return await GetById(id);
    }

    public async Task<ProductEntity?> Delete(Guid id)
    {
        var product = await GetById(id);
        if (product == null) return null;

        var result = await _collection.DeleteOneAsync(p => p.Id.Equals(product.Id));

        return result.DeletedCount == 0 ? null : product;
    }
    
    public async Task<ProductEntity?> AddTechnicalSpecs(Guid productId, List<TechnicalSpecEntity> specs)
    {
        var updateDefinition = Builders<ProductEntity>.Update.PushEach(p => p.TechnicalSpecs, specs);

        var updateResult = await _collection.UpdateOneAsync(
            p => p.Id.Equals(productId),
            updateDefinition
        );

        if (updateResult.MatchedCount == 0 || updateResult.ModifiedCount == 0)
        {
            return null;
        }

        return await GetById(productId);
    }
    
    public async Task<ProductEntity?> UpdateTechnicalSpecValue(Guid productId, Guid specTemplateId, string newValue)
    {
        var updateResult = await _collection.UpdateOneAsync(
            p => p.Id.Equals(productId) && p.TechnicalSpecs.Any(s => s.Id.Equals(specTemplateId)),
            Builders<ProductEntity>.Update
                .Set("TechnicalSpecs.$.Value", newValue)
        );

        if (updateResult.MatchedCount == 0 || updateResult.ModifiedCount == 0) return null;

        return await GetById(productId);
    }
}