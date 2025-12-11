using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Brands;
using store_api.Entities;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace store_api.Repositories;

public class BrandsRepository : IBaseRepository<BrandEntity>
{
    private static HttpClient _database;
    private readonly IConfiguration _configuration;
    
    private static readonly JsonSerializerOptions JsonOptions = new JsonSerializerOptions
    {
        Converters = { new JsonStringEnumConverter() },
        PropertyNameCaseInsensitive = true,
    };

    public BrandsRepository(IConfiguration configuration)
    {
        var clientHandler = new HttpClientHandler();
        clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
        _database = new(clientHandler);
        _configuration = configuration;
    }
    
    public async Task<BrandEntity?> Add(BrandEntity entity)
    {

        var content = JsonSerializer.Serialize(entity);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        String? mongoBaseUrl = _configuration.GetValue<String>("MongoBaseUrl") + "/brands";

        if (mongoBaseUrl == null)
        {
            throw new Exception("MongoBaseUrl not set");
        }
        
        var response = await _database.PostAsync(new Uri(mongoBaseUrl), httpContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<BrandEntity>(responseBody, JsonOptions);
        }

        return null;
    }
    
    public async Task<BrandEntity?> Update(Guid id, IBaseDto<BrandEntity> dto)
    {
        
        var updateDto = dto as BrandUpdateDto<BrandEntity>;
        if (updateDto == null) return null;
        
        var content = JsonSerializer.Serialize(updateDto);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        
        String mongoBaseUrl = _configuration.GetValue<String>("MongoBaseUrl") + $"/brands/{id}";
        var response = await _database.PutAsync(new Uri(mongoBaseUrl), httpContent);

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            return JsonSerializer.Deserialize<BrandEntity>(responseBody, JsonOptions);
        }

        return null;
    }

    public async Task<BrandEntity?> Delete(Guid id)
    {
        String mongoBaseUrl = _configuration.GetValue<String>("MongoBaseUrl") + $"/brands/{id}";
        var response = await _database.DeleteAsync(new Uri(mongoBaseUrl));

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BrandEntity>(responseBody, JsonOptions);
        }

        return null;
    }

    public async Task<BrandEntity?> GetById(Guid id)
    {
        String mongoBaseUrl = _configuration.GetValue<String>("MongoBaseUrl") + $"/brands/{id}";
        var response = await _database.GetAsync(new Uri(mongoBaseUrl));

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<BrandEntity>(responseBody, JsonOptions);
        }

        return null;
    }

    public async Task<IEnumerable<BrandEntity>?> GetAll()
    {
        String mongoBaseUrl = _configuration.GetValue<String>("MongoBaseUrl") + "/brands";
        var response = await _database.GetAsync(new Uri(mongoBaseUrl));

        if (response.IsSuccessStatusCode)
        {
            var responseBody = await response.Content.ReadAsStringAsync();
            
            var deserialized = JsonSerializer.Deserialize<List<BrandEntity>>(responseBody, JsonOptions);
            
            return deserialized;
        }

        return null;
    }
}