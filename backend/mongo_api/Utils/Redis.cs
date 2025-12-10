
using System.Net;
using System.Text.Json;
using System.Web;
using mongo_api.Entities;

namespace mongo_api.Utils;

public class Redis
{
    private static string? _getUri;
    private static string? _setUri;

    public Redis(IConfiguration configuration)
    {
        _getUri = configuration.GetValue<string>("Redis:Get");
        _setUri = configuration.GetValue<string>("Redis:Set");
    }
    
    private static async Task<T?> Get<T>(string key) where T : class
    {
        try
        {
            var uri = new Uri($"{_getUri}{key}");
            
            using var client = new HttpClient();
            var result = await client.GetAsync(uri);

            if (result.StatusCode == HttpStatusCode.NotFound) return null;
        
            result.EnsureSuccessStatusCode(); 
            
            return await result.Content.ReadFromJsonAsync<T>();
        }
        catch (Exception e)
        {
            return null;
        }
  
    }
    
    private static async Task Set<T>(string key, T value)
    {
        try
        {
            string serializedValue = JsonSerializer.Serialize(value);

            string encodedValue = HttpUtility.UrlEncode(serializedValue);

            var uri = new Uri($"{_setUri}{key}?value={encodedValue}");

            using var client = new HttpClient();
            var result = await client.PostAsync(uri, null);

            result.EnsureSuccessStatusCode();
        }
        catch (Exception e)
        {
            // Ignore :D
        }
    }
    
    public async Task<TEntity?> GetOrSetCache<TEntity>(string key, Func<Task<TEntity>> callback) where TEntity : class, IBaseEntity
    {
        var cachedEntity = await Get<TEntity>(key);
    
        if (cachedEntity == null)
        {
            TEntity newEntity = await callback();
            await Set(key, newEntity);
            return newEntity;
        }
    
        return cachedEntity;
    }

    public async Task<List<TItem>?> GetOrSetCache<TItem>(string key, Func<Task<List<TItem>>> callback) where TItem : class
    {
        var cachedList = await Get<List<TItem>>(key);
        
        if (cachedList == null)
        {
            List<TItem> newList = await callback();
            await Set(key, newList);
            return newList;
        }
        
        return cachedList;
    }
}