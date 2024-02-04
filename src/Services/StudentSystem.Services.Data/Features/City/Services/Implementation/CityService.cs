namespace StudentSystem.Services.Data.Features.City.Services.Implementation
{
    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Abstaction.Services;
    using StudentSystem.Services.Data.Features.City.Services.Contracts;

    public class CityService : BaseService<City>, ICityService
    {
        public CityService(IRepository<City> repository, IMapper mapper)
            : base(repository, mapper)
        {

        }

        public async Task<IEnumerable<TEntity>> GetAllAsync<TEntity>()
            => await Repository
                .AllAsNoTracking()
                .OrderBy(c => c.Name)
                .ProjectTo<TEntity>(Mapper.ConfigurationProvider)
                .ToListAsync();
    }
}
