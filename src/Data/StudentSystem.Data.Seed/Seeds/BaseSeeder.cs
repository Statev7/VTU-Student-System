namespace StudentSystem.Data.Seed.Seeds
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;

    using StudentSystem.Data.Seed.Contracts;

    public abstract class BaseSeeder<TEntity> : ISeeder
        where TEntity : class
    {
        protected BaseSeeder(IServiceScope serviceScope, string jsonData)
        {
            this.DbContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            this.DbSet = this.DbContext.Set<TEntity>();

            this.JsonData = jsonData;
        }

        protected ApplicationDbContext DbContext { get; }

        protected DbSet<TEntity> DbSet { get; }

        protected string JsonData { get; }

        public async Task<bool> IsAlreadySeedAsync()
            => await this.DbSet.AnyAsync();

        public abstract Task SeedAsync();
    }
}
