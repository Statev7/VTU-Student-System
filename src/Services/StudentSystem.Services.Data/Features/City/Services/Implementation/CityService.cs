namespace StudentSystem.Services.Data.Features.City.Services.Implementation
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common.Infrastructure.Cache.Services.Contracts;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.City.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    public class CityService : BaseService<City>, ICityService
    {
        private const string CacheKey = nameof(City);
        private readonly TimeSpan CacheTimeInDays = TimeSpan.FromDays(30);

        private readonly ICacheService cacheService;

        public CityService(
            IRepository<City> repository,
            IMapper mapper,
            ICacheService cacheService)
            : base(repository, mapper)
            => this.cacheService = cacheService;

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            => await this.cacheService.GetAsync(
                CacheKey,
                async () =>
                {
                    return await Repository
                        .AllAsNoTracking()
                        .OrderBy(c => c.Name)
                        .ProjectTo<TEntity>(Mapper.ConfigurationProvider)
                        .ToListAsync();
                }, 
                CacheTimeInDays);
    }
}
