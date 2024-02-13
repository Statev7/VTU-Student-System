namespace StudentSystem.Services.Data.Infrastructure.Collections.Contracts
{
    public interface IPageList<TEntity>
    {
        IEnumerable<TEntity> Entities { get; }

        bool HasEntities { get; }

        int CurrentPage { get; }

        int EntitiesPerPage { get; }

        int TotalPages { get; }

        int TotalEntities { get; }

        bool HasNextPage { get; }

        bool HasPreviousPage { get; }
    }
}
