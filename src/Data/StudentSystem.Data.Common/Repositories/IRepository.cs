namespace StudentSystem.Data.Common.Repositories
{
    using StudentSystem.Data.Common.Models;

    public interface IRepository<TEntity> : IDisposable
         where TEntity : class, IAuditInfo
    {
        IQueryable<TEntity> All();

        IQueryable<TEntity> AllAsNoTracking();

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        Task<TEntity> FindAsync<T>(T id);

        Task<int> SaveChangesAsync();
    }
}
