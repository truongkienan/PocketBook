using System.Runtime.CompilerServices;
using System.Text.Json;
using Microsoft.Extensions.Caching.Distributed;
using RedisCaching.Configurations;
using StackExchange.Redis;

namespace RedisCaching.Services
{
    public class ResponseCacheService : IResponseCacheService
    {
        IDatabase _cacheDb;
        private readonly IDistributedCache _distributedCache;
        private readonly IConnectionMultiplexer _connectionMultiplexer;

        public ResponseCacheService(bool enable, string connectionString)
        {
            Console.WriteLine($"--> Connecting redis server...");
            RedisConfiguration.Enabled = enable;
            RedisConfiguration.ConnectionString = connectionString;
            _connectionMultiplexer = ConnectionMultiplexer.Connect(connectionString);
            _cacheDb = _connectionMultiplexer.GetDatabase();
        }

        public async Task<string> GetCacheAsync(string key)
        {
            var value = await _cacheDb.StringGetAsync(key);

            return string.IsNullOrWhiteSpace(value) ? string.Empty : value;
        }

        public async Task SetCacheAsync(string key, object value, DateTimeOffset expirationTime)
        {
            if (value == null) return;

            Console.WriteLine("--> Setting cache...");
            Console.WriteLine(JsonSerializer.Serialize(value));
            var expirtyTime = expirationTime.DateTime.Subtract(DateTime.Now);
            await _cacheDb.StringSetAsync(key, JsonSerializer.Serialize(value), expirtyTime);
        }

        public async Task RemoveCacheAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentNullException("Value cannot be null or whitespace");

            Console.WriteLine("--> Removing cache...");
            
            await foreach (var key in GetKeyAsync(pattern + "*"))
            {
                var _exist = await _cacheDb.KeyExistsAsync(key);

                if (_exist)
                    await _cacheDb.KeyDeleteAsync(key);
            }
        }

        private async IAsyncEnumerable<string> GetKeyAsync(string pattern)
        {
            if (string.IsNullOrWhiteSpace(pattern))
                throw new ArgumentException("Value cannot be null or whitespace");

            foreach (var endPoint in _connectionMultiplexer.GetEndPoints())
            {
                var server = _connectionMultiplexer.GetServer(endPoint);
                foreach (var key in server.Keys(pattern: pattern))
                {
                    yield return key.ToString();
                }
            }
        }

    }
}
