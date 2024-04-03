namespace StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels
{
    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;

    public class ListCoursesViewModel<TEntity>
    {
        public IPageList<TEntity> PageList { get; set; }

        public CoursesRequestDataModel RequestData { get; set; }
    }
}
