namespace StudentSystem.Services.Data.Features.Courses.Services.Implementation
{
    using AutoMapper;

    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class CourseService : BaseService<Course>, ICourseService
    {
        public CourseService(IRepository<Course> repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

        public async Task<Result> CreateAsync(CourseFormBidningModel model)
        {
            var courseToCreate = this.Mapper.Map<Course>(model);

            await this.Repository.AddAsync(courseToCreate);
            var result = await this.Repository.SaveChangesAsync();

            return result != 0 ? true : UnsuccessfullyCourseCreationErrorMessage;
        }
    }
}
