namespace StudentSystem.Data.Common.Repositories
{
    using Microsoft.EntityFrameworkCore.Storage;

    using StudentSystem.Data.Common.Models;

    public interface IRepository<TEntity> : IDisposable
         where TEntity : class, IAuditInfo
    {
        IQueryable<TEntity> All(bool includeDeleted = false);

        IQueryable<TEntity> AllAsNoTracking(bool includeDeleted = false);

        Task AddAsync(TEntity entity);

        void Update(TEntity entity);

        void Delete(TEntity entity);

        void HardDelete(TEntity entity);

        void Undelete(TEntity entity);

        Task<TEntity> FindAsync<T>(T id);

        Task<int> SaveChangesAsync();

        Task<IDbContextTransaction> BeginTransactionAsync();
    }
}
