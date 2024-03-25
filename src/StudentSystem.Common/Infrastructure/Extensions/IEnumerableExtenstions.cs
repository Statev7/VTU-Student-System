namespace StudentSystem.Common.Infrastructure.Extensions
{
    public static class IEnumerableExtenstions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
            => collection == null || (collection != null && !collection.Any());
    }
}
