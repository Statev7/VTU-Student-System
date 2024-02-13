namespace StudentSystem.Services.Data.Features.Students.Services.Implementation
{
    using System;
    using System.Linq.Expressions;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;
    using StudentSystem.Services.Data.Infrastructure.Collections.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Extensions;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;

    public class StudentService : BaseService<Student>, IStudentService
    {
        private readonly ICurrentUserService currentUserService;

        public StudentService(
            IRepository<Student> repository, 
            IMapper mapper,
            ICurrentUserService currentUserService) 
            : base(repository, mapper)
        {
            this.currentUserService = currentUserService;
        }

        public async Task<IPageList<TEntity>> GetAllAsync<TEntity>(Expression<Func<Student, bool>> selector)
        {
            var students = await this.Repository
                .AllAsNoTracking()
                .Where(selector)
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToPagedAsync(1, 5);

            return students;
        }

        public async Task CreateAsync(BecomeStudentBindingModel model)
        {
            var studentToCreate = this.Mapper.Map<Student>(model);

            studentToCreate.ApplicationUserId = this.currentUserService.GetUserId();
            studentToCreate.IsApplied = true;

            await this.Repository.AddAsync(studentToCreate);
            await this.Repository.SaveChangesAsync();
        }

        public async Task<bool> IsAppliedAlreadyAsync()
            => await this.Repository
            .AllAsNoTracking()
            .AnyAsync(s => s.ApplicationUserId == this.currentUserService.GetUserId() && s.IsApplied);
    }
}
