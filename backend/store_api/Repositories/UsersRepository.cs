using System.Runtime.InteropServices.JavaScript;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using mongo_api.Utils;
using store_api.Controllers;
using store_api.Dtos;
using store_api.Dtos.Users;
using store_api.Entities;
using store_api.Exceptions;
using store_api.Utils;

namespace store_api.Repositories;

public class UsersRepository: IBaseRepository<UserEntity>
{
    private readonly string _mongoBaseUrl;
    private readonly HttpClient _client;
    private readonly JsonSerializerOptions _jsonOptions;

    public UsersRepository(IConfiguration configuration)
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

    public async Task<UserEntity?> Add(UserEntity entity)
    {
        if (_mongoBaseUrl == null) throw new Exception("MongoBaseUrl not set");

        var content = JsonSerializer.Serialize(entity);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PostAsync(new Uri(_mongoBaseUrl + "/users"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserEntity>(responseBody, _jsonOptions);
    }

    public async Task<UserEntity?> Update(Guid id, IBaseDto<UserEntity> dto)
    {
        var updateDto = dto as UserUpdateDto<UserEntity>;
        if(updateDto == null) throw new InvalidDtoType("Invalid data transfer object type.");

        var content = JsonSerializer.Serialize(updateDto);
        var httpContent = new StringContent(content, Encoding.UTF8, "application/json");

        var response = await _client.PutAsync(new Uri(_mongoBaseUrl + $"/users/{id}"), httpContent);
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserEntity>(responseBody, _jsonOptions);
    }

    public async Task<UserEntity?> Delete(Guid id)
    {
        var response = await _client.DeleteAsync(new Uri(_mongoBaseUrl + $"/users/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserEntity>(responseBody, _jsonOptions);
    }

    public async Task<UserEntity?> GetById(Guid id)
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + $"/users/{id}"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<UserEntity>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<UserEntity>?> GetAll()
    {
        var response = await _client.GetAsync(new Uri(_mongoBaseUrl + "/users"));
        if (!response.IsSuccessStatusCode) return null;

        var responseBody = await response.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<List<UserEntity>>(responseBody, _jsonOptions);
    }

    public async Task<IEnumerable<UserEntity>?> GetAllByName(string? firstName, string? lastName)
    {
        var result = await GetAll();
        if (result == null) return null;

        var isFNameNullOrEmpty = string.IsNullOrEmpty(firstName);
        var isLNameNullOrEmpty = string.IsNullOrEmpty(lastName);

        switch (isFNameNullOrEmpty, isLNameNullOrEmpty)
        {
            case (false, false):
                return result.Where(u => u.FirstName.Equals(firstName) && u.LastName.Equals(lastName));
            case (false, true):
                return result.Where(u => u.FirstName.Equals(firstName));
            case (true, false):
                return result.Where(u => u.LastName.Equals(lastName));
            default: return null;
        }
    }

    public async Task<UserEntity?> GetByEmail(String email)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.FirstOrDefault(u => u.Email == email);
    }

    public async Task<UserEntity?> GetByUsername(String username)
    {
        var result = await GetAll();
        if (result == null) return null;

        return result.FirstOrDefault(u => u.Username == username);
    }

    public async Task<UserEntity?> Login(UserLoginDto<UserEntity> dto)
    {
        var loginDto = dto;
        if (loginDto == null) return null;

        var result = await GetAll();
        if (result == null) return null;

        UserEntity? user = null;
        
        if (loginDto.Type == LoginType.USERNAME)
            user = result.FirstOrDefault(u => u.Username == loginDto.Identifier);

        if (loginDto.Type == LoginType.EMAIL)
            user = result.FirstOrDefault(u => u.Email == loginDto.Identifier);

        if (user == null || user.Password != Crypto.ToHexString(loginDto.Password)) return null;

        return user;
    }
}