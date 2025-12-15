using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Orders;
using store_api.Entities;
using store_api.Exceptions;


namespace store_api.Repositories;

public class OrdersRepository : IBaseRepository<OrderEntity>
{
    private readonly string _mongoBaseUrl;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public OrdersRepository(IConfiguration configuration)
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

    public async Task<OrderEntity?> Add(OrderEntity entity)
    {
        if (_mongoBaseUrl == null) throw new Exception("MongoBaseUrl not set");

        var content = JsonSerializer.Serialize(entity);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync(new Uri(_mongoBaseUrl + "/orders"), httpContent);
        
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderEntity>(responseBody, _jsonOptions);
    }
    
    public async Task<OrderEntity?> Update(Guid id, IBaseDto<OrderEntity> dto)
    {
        var updateDto = dto as OrderUpdateDto<OrderEntity>;
        if(updateDto == null) throw new InvalidDtoType("Invalid data transfer object type.");

        var content = JsonSerializer.Serialize(updateDto);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/orders/{id}"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderEntity>(responseBody, _jsonOptions);
    }
    
    public async Task<OrderEntity?> Delete(Guid id)
    {
        var response = await _client.DeleteAsync(new Uri(_mongoBaseUrl + $"/orders/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderEntity>(responseBody, _jsonOptions);
    }

    public async Task<OrderEntity?> GetById(Guid id)
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + $"/orders/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<OrderEntity>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<OrderEntity>?> GetAll()
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + "/orders"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<OrderEntity>>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<OrderEntity>?> GetOrdersByUser(Guid userId)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.Where(o => o.UserId == userId);
    }

    public async Task<decimal> CalculateOrderTotal(Guid orderId)
    {
        var result = await GetById(orderId);
        if (result == null) return -1;

        decimal total = 0;

        foreach (var orderItem in result.OrderItems)
            total += orderItem.UnitPrice * orderItem.Quantity;

        return total;
    }
}