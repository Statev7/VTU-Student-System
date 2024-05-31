namespace StudentSystem.Common.Infrastructure.Extensions
{
    public static class IEnumerableExtenstions
    {
        public static bool IsNullOrEmpty<T>(this IEnumerable<T> collection)
            => collection == null || (collection != null && !collection.Any());

        public static IEnumerable<(T item, int index)> WithIndex<T>(this IEnumerable<T> self)
           => self.Select((item, index) => (item, index));
    }
}
