namespace StudentSystem.Services.Data.Infrastructure.Abstaction.Services
{
    using AutoMapper;

    using StudentSystem.Data.Common.Models;
    using StudentSystem.Data.Common.Repositories;

    public abstract class BaseService<TEntity>
        where TEntity : class, IAuditInfo
    {
        protected BaseService(IRepository<TEntity> repository, IMapper mapper)
        {
            Repository = repository;
            Mapper = mapper;
        }

        protected IRepository<TEntity> Repository { get; }

        protected IMapper Mapper { get; }
    }
}
