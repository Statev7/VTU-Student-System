namespace StudentSystem.Common.Infrastructure.Extensions
{
    using System.Linq.Expressions;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Common.Infrastructure.Collections.Implementation;

    public static class IQueryableExtensions
    {
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
            if (condition)
            {
                source = source.Where(filter);
            }

            return source;
        }
    }
}
