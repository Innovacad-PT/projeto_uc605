using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Categories;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class CategoriesRepository: IBaseRepository<CategoryEntity>
{
    private static string _mongoBaseUrl;
    private static HttpClient _client;
    private static JsonSerializerOptions _jsonOptions;

    public CategoriesRepository(IConfiguration configuration)
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

    public async Task<CategoryEntity?> Add(CategoryEntity entity)
    {
        if (_mongoBaseUrl == null) throw new Exception("MongoBaseUrl not set");

        var content = JsonSerializer.Serialize(entity);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        Console.WriteLine(_mongoBaseUrl + $"/categories");
        
        var response = await _client.PostAsync(new Uri(_mongoBaseUrl + "/categories"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        Console.WriteLine(response);

        var responseBody = await response.Content.ReadAsStringAsync();
        Console.WriteLine(responseBody);
        return JsonSerializer.Deserialize<CategoryEntity>(responseBody, _jsonOptions);
    }

    public async Task<CategoryEntity?> Update(Guid id, IBaseDto<CategoryEntity> dto)
    {
        var updateDto = dto as CategoryUpdateDto<CategoryEntity>;
        if(updateDto == null) throw new InvalidDtoType("Invalid data transfer object type.");

        var content = JsonSerializer.Serialize(updateDto);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        
        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/categories/{id}"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CategoryEntity>(responseBody, _jsonOptions);
    }

    public async Task<CategoryEntity?> Delete(Guid id)
    {
        var response = await _client.DeleteAsync(new Uri(_mongoBaseUrl + $"/categories/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CategoryEntity>(responseBody, _jsonOptions);
    }

    public async Task<CategoryEntity?> GetById(Guid id)
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + $"/categories/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<CategoryEntity>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<CategoryEntity>?> GetAll()
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + "/categories"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<CategoryEntity>>(responseBody, _jsonOptions);
    }
}