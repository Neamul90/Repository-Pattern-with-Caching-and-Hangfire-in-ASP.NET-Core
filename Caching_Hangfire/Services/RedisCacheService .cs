﻿using Caching_Hangfire.Interface;

namespace Caching_Hangfire.Services
{
    public class RedisCacheService : ICacheService
    {
        public void Remove(string cacheKey)
        {
            throw new NotImplementedException();
        }
        public T Set<T>(string cacheKey, T value)
        {
            throw new NotImplementedException();
        }
        public bool TryGet<T>(string cacheKey, out T value)
        {
            throw new NotImplementedException();
        }
    }
}
