namespace StudentSystem.Common.Infrastructure.Extensions
{
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Common.Infrastructure.Collections.Implementation;

    public static class IQueryableExtensions
    {
        private const int OrderExpectedLength = 2;
        private const char OrderSeparator = '-';
        private const string DescOrder = "Desc";

        public static async Task<IPageList<TEntity>> ToPagedAsync<TEntity>(this IQueryable<TEntity> source, int currentPage, int entitiesPerPage)
        {
            if (source == null)
            {
                return PageList<TEntity>.CreateModel(new List<TEntity>(), currentPage, entitiesPerPage, 0);
            }

            currentPage = Math.Max(currentPage, 1);
            entitiesPerPage = Math.Max(entitiesPerPage, 1);

            var totalEntities = await source.CountAsync();

            var entities = await source
                .Skip((currentPage - 1) * entitiesPerPage)
                .Take(entitiesPerPage)
                .ToListAsync();

            var pageList = PageList<TEntity>.CreateModel(entities, currentPage, entitiesPerPage, totalEntities);

            return pageList;
        }

        public static IQueryable<TEntity> WhereIf<TEntity>(this IQueryable<TEntity> source, bool condition, Expression<Func<TEntity, bool>> filter)
        {
            if (source == null)
            {
                return Enumerable.Empty<TEntity>().AsQueryable();
            }

            if (condition)
            {
                source = source.Where(filter);
            }

            return source;
        }

        public static IQueryable<TEntity> OrderBy<TEntity>(this IQueryable<TEntity> source, string orderBy)
        {
            if (source == null)
            {
                return Enumerable.Empty<TEntity>().AsQueryable();
            }

            if (string.IsNullOrWhiteSpace(orderBy))
            {
                return source;
            }

            var sortCriteria = orderBy.Split(OrderSeparator, StringSplitOptions.TrimEntries);
            if (sortCriteria.Length != OrderExpectedLength)
            {
                return source;
            }

            var sortDirection = sortCriteria[0];
            var sortBy = sortCriteria[1];

            if (typeof(TEntity).GetProperty(sortBy) == null)
            {
                return source;
            }

            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, sortBy);
            var lambda = Expression.Lambda(property, parameter);

            var methodName = sortDirection == DescOrder ? nameof(Enumerable.OrderByDescending) : nameof(Enumerable.OrderBy);
            var methodCallExpression = Expression.Call(
                typeof(Queryable),
                methodName,
                new[] { typeof(TEntity), property.Type },
                source.Expression,
                Expression.Quote(lambda)
            );

            return source.Provider.CreateQuery<TEntity>(methodCallExpression);
        }
    }
}
