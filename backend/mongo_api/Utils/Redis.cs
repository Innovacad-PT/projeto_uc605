
using System.Text.Json;
using mongo_api.Entities;

namespace mongo_api.Utils;

public class Redis
{
    private static Task<T?> Get<T>(string key)
    {
        // TODO: Ir buscar os dados à API do REDIS.
        string? cachedValue = null;
        if (string.IsNullOrEmpty(cachedValue)) return Task.FromResult<T?>(default); 

        try
        {
            var result = JsonSerializer.Deserialize<T>(cachedValue);
            return Task.FromResult(result);
        }
        catch (Exception ex)
        {
            return Task.FromResult<T?>(default);
        }
    }
    
    private static void Set<T>(string key, T value)
    {
        // TODO: Ir buscar os dados à API do REDIS.
        string serializedValue = JsonSerializer.Serialize(value);
    }
    
    public async Task<TEntity?> GetOrSetCache<TEntity>(string key, Func<Task<TEntity>> callback) where TEntity : IBaseEntity 
    {
        var result = await Get<TEntity>(key);
        
        if (result == null)
        {
            result = await callback();
            Set(key, result);
        }
        
        return result;
    }
    public async Task<List<TItem>> GetOrSetCache<TItem>(string key, Func<Task<List<TItem>>> callback)
    {
        var result = await Get<List<TItem>>(key); 

        if (result == null)
        {
            result = await callback();
            Set(key, result); 
        }

        return result;
    }
}