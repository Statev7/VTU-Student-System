namespace StudentSystem.Services.Data.Features.City.Services.Implementation
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.City.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    public class CityService : BaseService<City>, ICityService
    {
        private const string CacheKey = nameof(City);
        private readonly TimeSpan CacheTime = TimeSpan.FromDays(30);

        private readonly IMemoryCache memoryCache;

        public CityService(
            IRepository<City> repository,
            IMapper mapper,
            IMemoryCache memoryCache)
            : base(repository, mapper) => this.memoryCache = memoryCache;

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
        {
            var cities = (IEnumerable<TEntity>)this.memoryCache.Get(CacheKey);

            if (cities == null)
            {
                cities = await Repository
                    .AllAsNoTracking()
                    .OrderBy(c => c.Name)
                    .ProjectTo<TEntity>(Mapper.ConfigurationProvider)
                    .ToListAsync();

                this.memoryCache.Set(CacheKey, cities, CacheTime);
            }

            return cities;
        }
    }
}
