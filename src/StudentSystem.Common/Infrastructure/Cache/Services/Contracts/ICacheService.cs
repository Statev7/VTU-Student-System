namespace StudentSystem.Common.Infrastructure.Cache.Services.Contracts
{
    public interface ICacheService
    {
        T? Get<T>(string key) 
            where T : class;

        public Task<T> GetAsync<T>(string key, Func<Task<T>> factory, TimeSpan duration = default)
            where T : class;

        void Set<T>(string key, T value, TimeSpan duration = default) 
            where T : class;

        void Remove(string key);

        void RemoveByPrefixOrSuffix(string prefix = null, string suffix = null);
    }
}
