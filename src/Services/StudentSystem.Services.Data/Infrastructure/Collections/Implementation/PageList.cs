namespace StudentSystem.Services.Data.Infrastructure.Collections.Implementation
{
    using StudentSystem.Services.Data.Infrastructure.Collections.Contracts;

    public class PageList<TEntity> : IPageList<TEntity>
    {
        private PageList(
            IEnumerable<TEntity> entities,
            int currentPage,
            int entitiesPerPage,
            int totalEntities)
        {
            Entities = entities;
            CurrentPage = currentPage;
            EntitiesPerPage = entitiesPerPage;
            TotalEntities = totalEntities;
        }

        public IEnumerable<TEntity> Entities { get; private set; }

        public int CurrentPage { get; private set; }

        public int EntitiesPerPage { get; private set; }

        public int TotalEntities { get; private set; }

        public int TotalPages => TotalEntities > 0
            ? (int)Math.Ceiling((double)(TotalEntities / (double)EntitiesPerPage))
            : 0;

        public bool HasPreviousPage => CurrentPage > 1;

        public bool HasNextPage => CurrentPage < TotalPages;

        public bool HasEntities => this.Entities != null && this.Entities.Any();

        public bool HasPagination => this.TotalPages > 1;

        public static IPageList<TEntity> CreateModel(
            IEnumerable<TEntity> entities,
            int currentPage,
            int entitiesPerPage,
            int totalEntities)
            => new PageList<TEntity>(entities, currentPage, entitiesPerPage, totalEntities);
    }
}
