namespace StudentSystem.Web.Infrastructure.Helpers.Implementation
{
    using StudentSystem.Services.Data.Features.Courses.DTOs.RequestDataModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Courses.Enums;
    using StudentSystem.Services.Data.Features.Courses.Services.Contracts;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Students.Services.Contracts;
    using StudentSystem.Services.Data.Infrastructure.Services.Contracts;
    using StudentSystem.Web.Infrastructure.Helpers.Contracts;
    using StudentSystem.Web.Models;

    public class HomeHelper : IHomeHelper
    {
        private const int CoursesToDisplay = 6;

        private readonly ICurrentUserService currentUserService;
        private readonly ICourseService courseService;
        private readonly IStudentService studentService;

        public HomeHelper(
            ICurrentUserService currentUserService,
            ICourseService courseService,
            IStudentService studentService)
        {
            this.currentUserService = currentUserService;
            this.courseService = courseService;
            this.studentService = studentService;
        }

        public async Task<HomeViewModel> CreateViewModelAsync()
        {
            var viewModel = new HomeViewModel();

            var userId = this.currentUserService.GetUserId();
            var isActiveStudent = !string.IsNullOrEmpty(userId) && await this.studentService.IsActiveAsync(userId);

            if (isActiveStudent)
            {
                viewModel.StudentDashboard = new StudentDashboardViewModel()
                {
                    Courses = await this.studentService.GetCoursesAsync<StudentCourseViewModel>(userId),
                    Schedule = await this.studentService.GetScheduleAsync<LessonScheduleViewModel>(userId),
                };
            }

            viewModel.IsActiveStudent = isActiveStudent;
            viewModel.CoursesSlider = await GetLatestCoursesAsync();

            return viewModel;
        }

        private async Task<IEnumerable<LatestCourseViewModel>> GetLatestCoursesAsync()
        {
            var requestData = new CoursesRequestDataModel() 
            { 
                CurrentPage = 1, 
                OrderBy = CoursesOrderOptions.DescStartDate 
            };

            var coursesPageList = await this.courseService.GetAllAsync<LatestCourseViewModel>(requestData, CoursesToDisplay);

            return coursesPageList.PageList.Entities;
        }
    }
}
