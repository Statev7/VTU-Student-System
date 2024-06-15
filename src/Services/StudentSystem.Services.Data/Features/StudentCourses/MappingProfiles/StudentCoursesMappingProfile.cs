namespace StudentSystem.Services.Data.Features.StudentCourses.MappingProfiles
{
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    public class StudentCoursesMappingProfile : BaseMappingProfile
    {
        public StudentCoursesMappingProfile()
        {
            this.CreateMap<CourseStudentMap, StudentWithGradeViewModel>()
                .ForMember(s => s.Id, conf => conf.MapFrom(s => s.StudentId))
                .ForMember(s => s.FirstName, conf => conf.MapFrom(s => s.Student.User.FirstName))
                .ForMember(s => s.LastName, conf => conf.MapFrom(s => s.Student.User.LastName))
                .ForMember(s => s.Grade, conf => conf.MapFrom(s => s.Exam.Grade));
        }
    }
}
