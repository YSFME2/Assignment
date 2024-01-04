

using System.Runtime.Caching;

namespace Web.Api.Services
{
    public class CacheService : ICacheService
    {
        private readonly MemoryCache _memoryCache = MemoryCache.Default;

        public T Get<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
                return (T)_memoryCache.Get(key);
            return default(T);
        }

        public bool Set<T>(string key, T value, DateTimeOffset expiration)
        {
            if (!string.IsNullOrEmpty(key))
                _memoryCache.Set(key, value, expiration);
            return false;
        }
        public void Delete<T>(string key)
        {
            if (!string.IsNullOrEmpty(key))
                _memoryCache.Remove(key);
        }
    }
}
