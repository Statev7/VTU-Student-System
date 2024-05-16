namespace StudentSystem.Data.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore.Storage;

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

        public virtual IQueryable<TEntity> All(bool includeDeleted = false) => this.DbSet.Where(x => x.IsDeleted.Equals(includeDeleted));

        public virtual IQueryable<TEntity> AllAsNoTracking(bool includeDeleted = false) => this.DbSet.AsNoTracking().Where(x => x.IsDeleted.Equals(includeDeleted));

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

        public void Delete(TEntity entity)
        {
            entity.IsDeleted = true;
            entity.DeletedOn = DateTime.UtcNow;
            this.Update(entity);
        }

        public void HardDelete(TEntity entity) => this.DbSet.Remove(entity);

        public void Undelete(TEntity entity)
        {
            entity.IsDeleted = false;
            entity.DeletedOn = null;
            this.Update(entity);
        }

        public Task<int> SaveChangesAsync() => this.DbContext.SaveChangesAsync();

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await this.DbContext.Database.BeginTransactionAsync();

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
