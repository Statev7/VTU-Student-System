namespace StudentSystem.Common.Infrastructure.Contracts
{
    public interface IHasPaging
    {
        int CurrentPage { get; }

        int EntitiesPerPage { get; }

        int TotalPages { get; }

        int TotalEntities { get; }

        bool HasNextPage { get; }

        bool HasPreviousPage { get; }
    }
}
