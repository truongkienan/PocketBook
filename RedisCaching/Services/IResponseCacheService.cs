using RedisCaching.Configurations;

namespace RedisCaching.Services
{
    public interface IResponseCacheService
    {

        Task<string> GetCacheAsync(string key);
        Task SetCacheAsync(string key, object value, DateTimeOffset expirationTime);
        Task RemoveCacheAsync(string pattern);
    }
}




