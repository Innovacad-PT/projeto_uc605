using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace store_api.Repositories;

public class ProductsRepository : IBaseRepository<ProductEntity>
{
    private readonly string _mongoBaseUrl;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;
    private readonly IConfiguration _configuration;

    public ProductsRepository(IConfiguration configuration)
    {
        _configuration = configuration;
        _mongoBaseUrl = configuration.GetValue<string>("MongoBaseUrl")!;
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        _client = new HttpClient(clientHandler);
        _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true
        };
    }

    public async Task<ProductEntity?> Add(ProductEntity entity)
    {
        if (_mongoBaseUrl == null) throw new Exception("MongoBaseUrl not set");

        Dictionary<String, dynamic> content = entity.ToJson();

        var httpContent = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(new Uri(_mongoBaseUrl + "/products"), httpContent);
        Console.WriteLine(JsonSerializer.Serialize(content));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        Dictionary<String, dynamic> json = JsonSerializer.Deserialize<Dictionary<String, dynamic>>(responseBody, _jsonOptions);
        
        return await ProductEntity.FromJson(_configuration, json);
    }
    
    public async Task<ProductEntity?> Update(Guid id, IBaseDto<ProductEntity> entity)
    {
        var updateDto = entity as ProductUpdateDto<ProductEntity>;
        if(updateDto == null) return null;

        var content = JsonSerializer.Serialize(updateDto, _jsonOptions);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/products/{id}"), httpContent);
        Console.WriteLine(content);
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        Dictionary<string, dynamic>? dict = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseBody, _jsonOptions);

        if (dict == null)
        {
            return null;
        }
        
        return await ProductEntity.FromJson(_configuration,  dict);
    }
    
    public async Task<ProductEntity?> UpdateWithImage(Guid id, ProductEntity entity)
    {
        var content = JsonSerializer.Serialize(entity.ToJson(), _jsonOptions);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/products/{id}"), httpContent);
        Console.WriteLine(content);
        Console.WriteLine(response.Content.ReadAsStringAsync().Result);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        Dictionary<string, dynamic>? dict = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseBody, _jsonOptions);

        if (dict == null)
        {
            return null;
        }
        
        return await ProductEntity.FromJson(_configuration,  dict);
    }

    public async Task<ProductEntity?> Delete(Guid id)
    {
        var response = await _client.DeleteAsync(new Uri(_mongoBaseUrl + $"/products/{id}"));
        
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        Dictionary<string, dynamic>? dict = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseBody, _jsonOptions);
        
        return await ProductEntity.FromJson(_configuration, dict);
    }

    public async Task<ProductEntity?> GetById(Guid id)
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + $"/products/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        Dictionary<string, dynamic>? dict = JsonSerializer.Deserialize<Dictionary<string, dynamic>>(responseBody, _jsonOptions);

        if (dict == null)
        {
            return null;
        }
        
        return await ProductEntity.FromJson(_configuration,  dict);
    }

    public async Task<IEnumerable<ProductEntity>?> GetAll()
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + "/products"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        
        var list = JsonSerializer.Deserialize<List<Dictionary<String, dynamic>>>(responseBody, _jsonOptions);
        var products = new List<ProductEntity>();
        
        Console.WriteLine(JsonSerializer.Serialize(list, _jsonOptions));

        foreach (var obj in list)
        {
            ProductEntity? prod = await ProductEntity.FromJson(_configuration, obj);

            if (prod is null)
            {
                continue;
            }
            
            products.Add(prod);
        }
        
        return products;
    }

    public async Task<IEnumerable<ProductEntity>?> GetAllWithFilters(string search, Guid categoryId, Guid brandId, decimal minPrice, decimal maxPrice)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.Where(p =>p.Name.ToLower()
            .ContainsAny(search.ToLower().AsSpan())
            && p.Id == categoryId
            && p.Id == brandId
            && (p.Price >= minPrice && p.Price <= maxPrice)
        );
    }

    public async Task<IEnumerable<ProductEntity>?> GetProductsInStock()
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.Where(p => p.Stock > 0);
    }

    public async Task<ProductEntity?> AddSpecs(Guid productId, List<Guid> specs)
    {
        var result = await GetById(productId);
        if (result == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }
        
        List<TechnicalSpecsEntity> existingSpecs = result.TechnicalSpecs ?? new List<TechnicalSpecsEntity>();
        HashSet<Guid> existingSpecIds = new(existingSpecs.Select(s => s.Id));
        
        var uniqueNewSpecs = specs.Distinct().ToList();
        uniqueNewSpecs.ForEach(s => existingSpecIds.Add(s));
        
        var updateDto = new ProductUpdateDto<ProductEntity>(
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            existingSpecIds.ToList(),
            null
        );

        var updateResult = await Update(productId, updateDto);
        if (updateResult == null) return null;

        return updateResult;
    }
    
    public async Task<ProductEntity?> RemoveSpec(Guid productId, Guid spec)
    {
        var result = await GetById(productId);
        if (result == null)
        {
            throw new KeyNotFoundException($"Product with ID {productId} not found.");
        }
        
        List<TechnicalSpecsEntity> existingSpecs = result.TechnicalSpecs ?? new List<TechnicalSpecsEntity>();
        HashSet<Guid> existingSpecIds = new(existingSpecs.Select(s => s.Id));
        
        existingSpecIds.Remove(spec);
        
        var updateDto = new ProductUpdateDto<ProductEntity>(
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            existingSpecIds.ToList(),
            null
        );

        var updateResult = await Update(productId, updateDto);
        if (updateResult == null) return null;

        return updateResult;
    }

    public async Task<ProductEntity?> IncreaseStock(Guid productId, int amount)
    {
        var result = await GetById(productId);
        if (result == null) return null;

        result.Stock += amount;

        var updateDto = new ProductUpdateDto<ProductEntity>(
            null,
            null,
            null,
            result.Stock,
            null,
            null,
            null,
            null,
            null
        );

        var updateResult = await Update(productId, updateDto);
        if (updateResult == null) return null;

        return updateResult;
    }

    public async Task<ProductEntity?> DecreaseStock(Guid productId, int amount)
    {
        var result = await GetById(productId);
        if (result == null) return null;

        result.Stock -= amount;

        var updateDto = new ProductUpdateDto<ProductEntity>(
            null,
            null,
            null,
            result.Stock,
            null,
            null,
            null,
            null,
            null
        );

        var updateResult = await Update(productId, updateDto);
        if (updateResult == null) return null;

        return updateResult;
    }

    public async Task<bool> CanCreateOrder(Guid productId, int quantity)
    {
        var result = await GetById(productId);
        if (result == null) return false;

        return result.Stock >= quantity;
    }
}