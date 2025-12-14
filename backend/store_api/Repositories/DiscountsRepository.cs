using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Discounts;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class DiscountsRepository : IBaseRepository<DiscountEntity>
{
    private readonly string _mongoBaseUrl;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public DiscountsRepository(IConfiguration configuration)
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

    public async Task<DiscountEntity?> Add(DiscountEntity entity)
    {
        if (_mongoBaseUrl == null) throw new Exception("MongoBaseUrl not set");

        var content = JsonSerializer.Serialize(entity);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(new Uri(_mongoBaseUrl + "/discounts"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DiscountEntity>(responseBody, _jsonOptions);
    }

    public async Task<DiscountEntity?> Update(Guid id, IBaseDto<DiscountEntity> dto)
    {
        var updateDto = dto as DiscountUpdateDto<DiscountEntity>;
        if(updateDto == null) throw new InvalidDtoType("Invalid data transfer object type.");

        var content = JsonSerializer.Serialize(updateDto);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/discounts/{id}"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DiscountEntity>(responseBody, _jsonOptions);
    }

    public async Task<DiscountEntity?> Delete(Guid id)
    {
        var response = await _client.DeleteAsync(new Uri(_mongoBaseUrl + $"/discounts/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DiscountEntity>(responseBody, _jsonOptions);
    }

    public async Task<DiscountEntity?> GetById(Guid id)
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + $"/discounts/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<DiscountEntity>(responseBody, _jsonOptions);
    }
    
    public async Task<IEnumerable<DiscountEntity>?> GetAll()
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + "/discounts"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<DiscountEntity>>(responseBody, _jsonOptions);
    }

    public async Task<DiscountEntity?> GetActiveDiscount(Guid productId)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.FirstOrDefault(d =>
            d.ProductId == productId &&
            d.StartDate <= DateTime.Now &&
            DateTime.Now < d.EndDate
        );
    }
}