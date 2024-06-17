namespace StudentSystem.Services.Data.Features.StudentCourses.MappingProfiles
{
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.StudentCourses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    public class StudentCoursesMappingProfile : BaseMappingProfile
    {
        public StudentCoursesMappingProfile()
        {
            this.CreateMap<CourseStudentMap, StudentWithGradeViewModel>()
                .ForMember(d => d.Id, conf => conf.MapFrom(s => s.StudentId))
                .ForMember(d => d.FirstName, conf => conf.MapFrom(s => s.Student.User.FirstName))
                .ForMember(d => d.LastName, conf => conf.MapFrom(s => s.Student.User.LastName))
                .ForMember(d => d.Grade, conf => conf.MapFrom(s => s.Exam.Grade));

            this.CreateMap<CourseStudentMap, StudentDashboardCourseViewModel>()
                .ForMember(d => d.Id, conf => conf.MapFrom(s => s.CourseId))
                .ForMember(d => d.Name, conf => conf.MapFrom(s => s.Course.Name))
                .ForMember(d => d.IsActive, conf => conf.MapFrom(s => s.Course.IsActive))
                .ForMember(d => d.ExamGrade, conf => conf.MapFrom(s => s.Exam.Grade));

            this.CreateMap<CourseStudentMap, CourseWithExamDetailsViewModel>()
                .ForMember(d => d.Id, conf => conf.MapFrom(s => s.CourseId))
                .ForMember(d => d.Name, conf => conf.MapFrom(s => s.Course.Name))
                .ForMember(d => d.StartDate, conf => conf.MapFrom(s => s.Course.StartDate))
                .ForMember(d => d.EndDate, conf => conf.MapFrom(s => s.Course.EndDate))
                .ForMember(d => d.Credits, conf => conf.MapFrom(s => s.Course.Credits))
                .ForMember(d => d.Grade, conf => conf.MapFrom(s => s.Exam.Grade))
                .ForMember(d => d.GradeComment, conf => conf.MapFrom(s => s.Exam.Comment ));
        }
    }
}
