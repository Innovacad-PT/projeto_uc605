using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Reviews;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class ReviewsRepository : IBaseRepository<ReviewEntity>
{
    private readonly string _mongoBaseUrl;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public ReviewsRepository(IConfiguration configuration)
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

    public async Task<ReviewEntity?> Add(ReviewEntity entity)
    {
        if (_mongoBaseUrl == null) throw new Exception("MongoBaseUrl not set");

        var content = JsonSerializer.Serialize(entity);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(new Uri(_mongoBaseUrl + "/reviews"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ReviewEntity>(responseBody, _jsonOptions);
    }

    public async Task<ReviewEntity?> Update(Guid id, IBaseDto<ReviewEntity> dto)
    {
        var updateDto = dto as ReviewUpdateDto<ReviewEntity>;
        if(updateDto == null) throw new InvalidDtoType("Invalid data transfer object type.");

        var content = JsonSerializer.Serialize(updateDto);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/reviews/{id}"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ReviewEntity>(responseBody, _jsonOptions);
    }

    public async Task<ReviewEntity?> Delete(Guid id)
    {
        var response = await _client.DeleteAsync(new Uri(_mongoBaseUrl + $"/reviews/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ReviewEntity>(responseBody, _jsonOptions);
    }

    public async Task<ReviewEntity?> GetById(Guid id)
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + $"/reviews/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<ReviewEntity>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<ReviewEntity>?> GetAll()
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + "/reviews"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<ReviewEntity>>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<ReviewEntity>?> GetReviewsByProduct(Guid productId)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.Where(r => r.ProductId == productId);;
    }

    public async Task<ReviewEntity?> GetUserReview(Guid userId, Guid productId)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.FirstOrDefault(r => r.UserId == userId && r.ProductId == productId);
    }
}