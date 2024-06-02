namespace StudentSystem.Common.Infrastructure.Cache.Services.Implementation
{
    using System.Linq;

    using Microsoft.Extensions.Caching.Memory;

    using StudentSystem.Common.Infrastructure.Cache.Services.Contracts;

    public class CacheService : ICacheService
    {
        private readonly Dictionary<string, bool> CacheKeys = new();
        private readonly IMemoryCache memoryCache;

        public CacheService(IMemoryCache memoryCache) 
            => this.memoryCache = memoryCache;

        public T? Get<T>(string key)
            where T : class
        {
            T? value = this.memoryCache.Get<T>(key);

            if (value == null)
            {
                return null;
            }

            return value;
        }

        public async Task<T> GetAsync<T>(string key, Func<Task<T>> factory, TimeSpan duration = default)
            where T : class
        {
            T? cachedValue = this.Get<T>(key);

            if (cachedValue != null) 
            {
                return cachedValue;
            }

            cachedValue = await factory();

            this.Set<T>(key, cachedValue, duration);

            return cachedValue;
        }

        public void Set<T>(string key, T value, TimeSpan duration = default)
            where T : class
        {
            this.memoryCache.Set<T>(key, value, duration);

            this.CacheKeys.TryAdd(key, false);
        }

        public void Remove(string key)
        {
            this.memoryCache.Remove(key);

            if (this.CacheKeys.ContainsKey(key))
            {
                this.CacheKeys.Remove(key);
            }
        }

        public void RemoveByPrefixOrSuffix(string prefix = null, string suffix = null)
        {
            var keysToDelete = this.CacheKeys.Keys
                .Where(x => !string.IsNullOrEmpty(prefix) && x.StartsWith(prefix) || 
                            !string.IsNullOrEmpty(suffix) && x.EndsWith(suffix));

            foreach (var key in keysToDelete)
            {
                this.memoryCache.Remove(key);
            }
        }
    }
}
