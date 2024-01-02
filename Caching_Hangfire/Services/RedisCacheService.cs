using Caching_Hangfire.Helper;
using Caching_Hangfire.Interface;
using MassTransit.Initializers.Variables;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Caching_Hangfire.Services
{
    public class RedisCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly CacheConfiguration _cacheConfig;
        private DistributedCacheEntryOptions _cacheOptions;
        private readonly IDistributedCache _distributedCache;

        public RedisCacheService(IDistributedCache distributedCache,IMemoryCache memoryCache, IOptions<CacheConfiguration> cacheConfig)
        {
            _memoryCache = memoryCache;
            _cacheConfig = cacheConfig.Value;
            this._distributedCache = distributedCache;
            if (_cacheConfig != null)
            {
                _cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTime.Now.AddHours(_cacheConfig.AbsoluteExpirationInHours),
                    SlidingExpiration = TimeSpan.FromMinutes(_cacheConfig.SlidingExpirationInMinutes)
                };
            }
        }
       
        public void Remove(string cacheKey)
        {
            _distributedCache.Remove(cacheKey);
        }
        
        public bool TryGet<T>(string cacheKey, out T value)
        {
            value = default(T);
            var data = _distributedCache.GetString(cacheKey);
            if(data != null)
            {
                var item = JsonSerializer.Deserialize<T>(data);
                value = (T)item;
            }
            if (value == null) return false;
            else return true;
        }

        public T Set<T>(string cacheKey, T value)
        {
            var jsonData = JsonSerializer.Serialize(value);
            _distributedCache.SetString(cacheKey, jsonData, _cacheOptions);
            return value;
        }
    }
}
