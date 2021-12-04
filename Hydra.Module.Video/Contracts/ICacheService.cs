using System;
using Microsoft.Extensions.Caching.Memory;

namespace Hydra.Module.Video.Contracts
{
    public interface ICacheService
    {
        public T GetFromCache<T>(string key) where T : class;

        public void SetCache<T>(string key, T value) where T : class;

        public void SetCache<T>(string key, T value, DateTimeOffset duration) where T : class;

        public void SetCache<T>(string key, T value, MemoryCacheEntryOptions options) where T : class;

        public void ClearCache(string key);
    }
}