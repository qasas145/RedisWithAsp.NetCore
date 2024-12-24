using System.Text.Json;
using Microsoft.Extensions.Options;
using my_redis.Data;
using my_redis.Options;
using StackExchange.Redis;

namespace my_redis.Services;

public class CacheService : ICacheService
{
    private readonly IDatabase _cacheDb;
    private readonly ConnectionStrings connectionStrings;
    public CacheService(IOptions<ConnectionStrings> options) {
        connectionStrings = options.Value;

        var redis = ConnectionMultiplexer.Connect(connectionStrings.RedisConnection);
        _cacheDb = redis.GetDatabase();
        Console.WriteLine("Connected to the redis server");

    }
    public async Task<T> GetData<T>(string key)
    {
        var value = await _cacheDb.StringGetAsync(key);
        if (!string.IsNullOrEmpty(value)) 
            return JsonSerializer.Deserialize<T>(value);
        
        return default;
    }

    public async Task<bool> SetData<T>(string key, T data) {
        return await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize<T>(data));
    }

    public async Task<bool> RemoveKey(string key)
    {
        if (await _cacheDb.KeyDeleteAsync(key)) 
            return true;
        
        return false;
    }

}
