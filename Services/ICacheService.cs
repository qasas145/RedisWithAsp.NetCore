namespace my_redis.Services;

public interface ICacheService
{
    Task<T> GetData<T>(string key);
    Task<bool> SetData<T>(string key, T data, DateTimeOffset expirationTime);
    Task<bool> RemoveKey(string key);
}
