using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.TechnicalSpecs;
using store_api.Entities;
using store_api.Exceptions;

namespace store_api.Repositories;

public class TechnicalSpecsRepository : IBaseRepository<TechnicalSpecsEntity>
{
    private readonly string _mongoBaseUrl;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public TechnicalSpecsRepository(IConfiguration configuration)
    {
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
    
    public async Task<TechnicalSpecsEntity?> Add(TechnicalSpecsEntity entity)
    {
        if (_mongoBaseUrl == null) throw new Exception("MongoBaseUrl not set");

        var content = JsonSerializer.Serialize(entity);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(new Uri(_mongoBaseUrl + "/techspecs"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TechnicalSpecsEntity>(responseBody, _jsonOptions);
    }

    public async Task<TechnicalSpecsEntity?> Update(Guid id, IBaseDto<TechnicalSpecsEntity> dto)
    {
        var updateDto = dto as TechnicalSpecsUpdateDto<TechnicalSpecsEntity>;
        if(updateDto == null) throw new InvalidDtoType("Invalid data transfer object type.");

        var content = JsonSerializer.Serialize(updateDto);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/techspecs/{id}"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TechnicalSpecsEntity>(responseBody, _jsonOptions);
    }

    public async Task<TechnicalSpecsEntity?> Delete(Guid id)
    {
        var response = await _client.DeleteAsync(new Uri(_mongoBaseUrl + $"/techspecs/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TechnicalSpecsEntity>(responseBody, _jsonOptions);
    }

    public async Task<TechnicalSpecsEntity?> GetById(Guid id)
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + $"/techspecs/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<TechnicalSpecsEntity>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<TechnicalSpecsEntity>?> GetAll()
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + "/techspecs"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<TechnicalSpecsEntity>>(responseBody, _jsonOptions);
    }

    public async Task<TechnicalSpecsEntity?> GetByKey(String key)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.FirstOrDefault((s) => s.Key.ToLower() == key.ToLower());
    }
}