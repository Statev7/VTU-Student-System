﻿namespace StudentSystem.Common.Infrastructure.Cache.Services.Contracts
{
    using StudentSystem.Common.Infrastructure.Cache.Settings;

    public interface ICacheService
    {
        T? Get<T>(string key) 
            where T : class;

        public Task<T> GetAsync<T>(string key, Func<Task<T>> factory, TimeSpan duration = default)
            where T : class;

        void Set<T>(string key, T value, TimeSpan duration = default) 
            where T : class;

        void Remove(string key);

        void RemoveByPrefix(string prefix);

        void RemoveByCollectionKeysPrefixes(CacheKeyCollection masterKey);
    }
}
