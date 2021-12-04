using System;
using Hydra.Module.Video.Contracts;
using Microsoft.Extensions.Caching.Memory;

namespace Hydra.Module.Video.Services
{
    public class CacheService : ICacheService
    {

        private const int CacheSeconds = 10;

        private readonly IMemoryCache _cache;

        public CacheService(IMemoryCache cache)
        {
            _cache = cache;
        }

        public T GetFromCache<T>(string key) where T : class
        {
            _cache.TryGetValue(key, out T cachedResponse);
            return cachedResponse;
        }

        public void SetCache<T>(string key, T value) where T : class
        {
            SetCache(key, value, DateTimeOffset.Now.AddSeconds(CacheSeconds));
        }

        public void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class
        {
            _cache.Set(key, value, duration);
        }

        public void SetCache<T>(string key, T value, MemoryCacheEntryOptions options) where T : class
        {
            _cache.Set(key, value, options);
        }

        public void ClearCache(string key)
        {
            _cache.Remove(key);
        }
    }
    
}