namespace StudentSystem.Services.Data.Features.Students.DTOs.ViewModels
{
    using StudentSystem.Common.Infrastructure.Collections.Contracts;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.RequestDataModels;

    public class ListStudentsViewModel
    {
        public IPageList<StudentWithGradeViewModel> StudentsPageList { get; set; }

        public StudentsInCourseRequestData RequestData { get; set; }
    }
}
