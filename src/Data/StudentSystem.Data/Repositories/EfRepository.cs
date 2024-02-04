namespace StudentSystem.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Common.Models;
    using StudentSystem.Data.Common.Repositories;

    public class EfRepository<TEntity> : IRepository<TEntity>
        where TEntity : class, IAuditInfo
    {
        public EfRepository(ApplicationDbContext dbContext)
        {
            this.DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
            this.DbSet = this.DbContext.Set<TEntity>();
        }

        protected DbSet<TEntity> DbSet { get; set; }

        protected ApplicationDbContext DbContext { get; set; }

        public virtual IQueryable<TEntity> All() => this.DbSet;

        public virtual IQueryable<TEntity> AllAsNoTracking() => this.DbSet.AsNoTracking();

        public virtual Task AddAsync(TEntity entity) => this.DbSet.AddAsync(entity).AsTask();

        public Task<TEntity> FindAsync<T>(T id) => this.DbSet.FindAsync(id).AsTask();

        public virtual void Update(TEntity entity)
        {
            var entry = this.DbContext.Entry(entity);
            if (entry.State == EntityState.Detached)
            {
                this.DbSet.Attach(entity);
            }

            entry.State = EntityState.Modified;
        }

        public virtual void Delete(TEntity entity) => this.DbSet.Remove(entity);

        public Task<int> SaveChangesAsync() => this.DbContext.SaveChangesAsync();

        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.DbContext?.Dispose();
            }
        }
    }
}
