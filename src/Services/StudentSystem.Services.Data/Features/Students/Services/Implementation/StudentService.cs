namespace StudentSystem.Services.Data.Features.Students.Services.Implementation
{
    using AutoMapper;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Abstaction.Services;
    using StudentSystem.Services.Data.Common;
    using StudentSystem.Services.Data.Common.Services.Contracts;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;

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

        public async Task<Result> CreateAsync(BecomeStudentBindingModel model)
        {
            var studentToCreate = this.Mapper.Map<Student>(model);
            studentToCreate.ApplicationUserId = this.currentUserService.GetUserId();
            studentToCreate.IsApplied = true;

            await this.Repository.AddAsync(studentToCreate);
            await this.Repository.SaveChangesAsync();

            return true;
        }

        public async Task<bool> IsAppliedAlreadyAsync()
            => await this.Repository
            .AllAsNoTracking()
            .AnyAsync(s => s.ApplicationUserId == this.currentUserService.GetUserId() && s.IsApplied);
    }
}
