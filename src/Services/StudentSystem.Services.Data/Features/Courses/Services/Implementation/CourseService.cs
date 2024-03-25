namespace StudentSystem.Services.Data.Features.Courses.Services.Implementation
{
    using System;

    using AutoMapper;
    using AutoMapper.QueryableExtensions;

    using Microsoft.EntityFrameworkCore;

    using StudentSystem.Common.Infrastructure.Extensions;
    using StudentSystem.Data.Common.Repositories;
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Services;

    using static StudentSystem.Common.Constants.NotificationConstants;

    public class CourseService : BaseService<Course>, ICourseService
    {
        private const int CoursesPerPage = 6;

        public CourseService(IRepository<Course> repository, IMapper mapper) 
            : base(repository, mapper)
        {
        }

        public async Task<ListCoursesViewModel<TEntity>> GetAllAsync<TEntity>(CoursesRequestDataModel requestData)
        {
            var pagedCourses = await this.Repository
                .AllAsNoTracking()
                .WhereIf(!string.IsNullOrEmpty(requestData.SearchTerm), x => x.Name.Contains(requestData.SearchTerm))
                .OrderBy(requestData.OrderBy.GetEnumValue())
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .ToPagedAsync(requestData.CurrentPage, CoursesPerPage);

            var resultModel = new ListCoursesViewModel<TEntity>() { PageList = pagedCourses, RequestData = requestData };

            return resultModel;
        }

        public async Task<TEntity> GetByIdAsync<TEntity>(Guid id)
            => await this.Repository.AllAsNoTracking()
                .Where(x => x.Id.Equals(id))
                .ProjectTo<TEntity>(this.Mapper.ConfigurationProvider)
                .FirstOrDefaultAsync();

        public async Task<Result> CreateAsync(CourseFormBindingModel bindingModel)
        {
            var courseToCreate = this.Mapper.Map<Course>(bindingModel);

            await this.Repository.AddAsync(courseToCreate);
            await this.Repository.SaveChangesAsync();

            return Result.Success(SuccessfullyCreatedCourseMessage);
        }

        public async Task<Result> UpdateAsync(Guid id, CourseFormBindingModel bindingModel)
        {
            var courseToUpdate = await this.Repository.FindAsync(id);

            if (courseToUpdate == null)
            {
                return InvalidCourseErrorMessage;
            }

            this.Mapper.Map(bindingModel, courseToUpdate);

            this.Repository.Update(courseToUpdate);
            await this.Repository.SaveChangesAsync();

            return Result.Success(SuccessfillyUpdatedCourseMessage);
        }

        public async Task<Result> DeleteAsync(Guid id)
        {
            var courseToDelete = await this.Repository.FindAsync(id);

            if (courseToDelete == null)
            {
                return InvalidCourseErrorMessage;
            }

            this.Repository.Delete(courseToDelete);
            await this.Repository.SaveChangesAsync();

            return Result.Success(SuccessfillyDeletedCourseMessage);
        }
    }
}
