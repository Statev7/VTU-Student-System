namespace StudentSystem.Common.Infrastructure.Cache.Settings
{
    public static class CacheKeyGenerator
    {
        public static string GenerateKey(string prefix, Type type, params CacheParameter[] parameters)
            => $"{prefix}:{type.Name}:{string.Join(":", parameters.Select(x => x.ToString()))}";

        public static string GenerateKey(string prefix, Guid id, Type type)
            => $"{prefix}:{id}:{type.Name}";
    }
}
