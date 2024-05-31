namespace StudentSystem.Common.Infrastructure.Cache.Settings
{
    public static class CacheKeyGenerator
    {
        public static string GenerateKey<T>(string prefix, params CacheParameter[] parameters)
            => $"{prefix}:{typeof(T).Name}:{string.Join(":", parameters.Select(x => x.ToString()))}";

        public static string GenerateKey<T>(string prefix, Guid id)
            => $"{prefix}:{typeof(T).Name}:{id}";

        public static string GenerateKey<T>(Guid id)
            => $"{typeof(T).Name}:{id}";
    }
}
