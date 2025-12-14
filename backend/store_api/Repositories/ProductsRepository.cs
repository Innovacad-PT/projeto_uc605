using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class ProductsRepository : IBaseRepository<ProductEntity>
{
    private readonly CategoriesRepository _categoriesRepository;
    private readonly BrandsRepository _brandsRepository;
    private readonly string _mongoBaseUrl;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public ProductsRepository(IConfiguration configuration)
    {
        _categoriesRepository = new(configuration);
        _brandsRepository = new(configuration);
        _mongoBaseUrl = configuration.GetValue<string>("MongoBaseUrl")!;
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        _client = new HttpClient(clientHandler);
        _jsonOptions = new JsonSerializerOptions
        {
            Converters = { new JsonStringEnumConverter() },
            PropertyNameCaseInsensitive = true,
        };
    }

    public async Task<ProductEntity?> Add(ProductEntity entity)
    {
        if (_mongoBaseUrl == null) throw new Exception("MongoBaseUrl not set");

        var content = JsonSerializer.Serialize(entity);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(new Uri(_mongoBaseUrl + "/products"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductEntity>(responseBody, _jsonOptions);
    }

    
    public async Task<ProductEntity?> Update(Guid id, IBaseDto<ProductEntity> dto)
    {
        var updateDto = dto as ProductUpdateDto<ProductEntity>;
        if(updateDto == null) throw new InvalidDtoType("Invalid data transfer object type.");

        var content = JsonSerializer.Serialize(updateDto);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/products/{id}"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductEntity>(responseBody, _jsonOptions);
    }

    public async Task<ProductEntity?> Delete(Guid id)
    {
        var response = await _client.DeleteAsync(new Uri(_mongoBaseUrl + $"/products/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductEntity>(responseBody, _jsonOptions);
    }

    public async Task<ProductEntity?> GetById(Guid id)
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + $"/products/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ProductEntity>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<ProductEntity>?> GetAll()
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + "/products"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ProductEntity>>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<ProductEntity>?> GetAllWithFilters(string search, Guid categoryId, Guid brandId, decimal minPrice, decimal maxPrice)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.Where(p =>p.Name.ToLower()
            .ContainsAny(search.ToLower().AsSpan())
            && p.Category.Id == categoryId
            && p.Brand.Id == brandId
            && (p.Price >= minPrice && p.Price <= maxPrice)
        );
    }

    public async Task<IEnumerable<ProductEntity>?> GetProductsInStock()
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.Where(p => p.Stock > 0);
    }

    public async Task<ProductEntity?> AddSpecs(Guid productId, List<ProductTechnicalSpecsEntity> specs)
    {
        var result = await GetById(productId);
        if (result == null) return null;

        List<ProductTechnicalSpecsEntity> existingSpecs = result.TechnicalSpecs;
        
        HashSet<Guid> existingSpecIds = new(existingSpecs.Select(s => s.TechnicalSpecsId));

        List<ProductTechnicalSpecsEntity> newSpecs = new();
        List<ProductTechnicalSpecsEntity> duplicateSpecs = new();

        foreach (var spec in specs)
        {
            spec.ProductId = productId;
            
            if (existingSpecIds.Contains(spec.TechnicalSpecsId))
            {
                duplicateSpecs.Add(spec); 
            }
            else
            {
                newSpecs.Add(spec);
            }
        }
        
        if (duplicateSpecs.Count > 0)
        {
            string duplicateKeys = string.Join(", ", duplicateSpecs.Select(d => d.Key));
            throw new InvalidOperationException($"Cannot add technical specs. The following spec keys already exist for product {productId}: {duplicateKeys}");
        }

        result.TechnicalSpecs.AddRange(newSpecs);

        var updateDto = new ProductUpdateDto<ProductEntity>(
            null,
            null,
            null,
            null,
            null,
            null,
            null,
            result.TechnicalSpecs,
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