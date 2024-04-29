namespace StudentSystem.Common.Infrastructure.Cache.Settings
{
    public static class CacheKeyGenerator
    {
        public static string GenerateKey(string region, Type type, params CacheParameter[] parameters)
            => $"{region}:{type.Name}:{string.Join(":", parameters.Select(x => x.ToString()))}";

        public static string GenerateKey(string region, Guid id, Type type)
            => $"{region}:{id}:{type.Name}";

        public static string GenerateRegionKey(string region, Guid id)
            => $"{region}:{id}";
    }
}
