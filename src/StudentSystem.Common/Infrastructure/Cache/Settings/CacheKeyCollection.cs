namespace StudentSystem.Common.Infrastructure.Cache.Settings
{
    public class CacheKeyCollection
    {
        public CacheKeyCollection(params string[] keys)
        {
            this.Keys = keys;
        }

        public IReadOnlyCollection<string> Keys { get; }
    }
}
