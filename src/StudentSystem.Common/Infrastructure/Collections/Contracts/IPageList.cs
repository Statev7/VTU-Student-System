namespace StudentSystem.Common.Infrastructure.Collections.Contracts
{
    using StudentSystem.Common.Infrastructure.Contracts;

    public interface IPageList<TEntity> : IHasPaging
    {
        IEnumerable<TEntity> Entities { get; }

        bool HasEntities { get; }

        bool HasPagination { get; }
    }
}
