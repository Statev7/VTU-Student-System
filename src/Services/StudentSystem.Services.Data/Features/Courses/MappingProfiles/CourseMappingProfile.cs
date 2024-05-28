namespace StudentSystem.Services.Data.Features.Courses.MappingProfiles
{
    using Microsoft.AspNetCore.Http;

    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Courses.Constants;
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Features.Courses.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;
    using StudentSystem.Services.Data.Infrastructure.StaticHelpers;

    public class CourseMappingProfile : BaseMappingProfile
    {
        public CourseMappingProfile()
        {
            this.CreateMap<CourseFormBindingModel, Course>()
                .ForMember(d => d.TeaserDescription, conf => conf.MapFrom(s => HtmlHelper.Sanitize(s.TeaserDescription)))
                .ForMember(d => d.Description, conf => conf.MapFrom(s => HtmlHelper.Sanitize(s.Description)))
                .ForMember(d => d.IsActive, conf => conf.MapFrom(_ => true)); ;

            this.CreateMap<Course, CourseFormBindingModel>();

            this.CreateMap<Course, CourseViewModel>()
                .ForMember(d => d.StartDate, conf => conf.MapFrom(s => s.StartDate.ToString(CourseConstants.DateFormat)))
                .ForMember(d => d.Duration, conf => conf.MapFrom(s => (int)Math.Ceiling((s.EndDate - s.StartDate).TotalDays / 7)))
                .ForMember(d => d.ImageUrl, conf => conf.MapFrom(s => s.ImageFolder));

            this.CreateMap<Course, LatestCourseViewModel>()
                .ForMember(d => d.StartDate, conf => conf.MapFrom(s => s.StartDate.ToString(CourseConstants.DateFormat)))
                .ForMember(d => d.ImageUrl, conf => conf.MapFrom(s => s.ImageFolder));

            this.CreateMap<Course, CoursePaymentDetailsServiceModel>();

            this.CreateMap<Course, CourseManagementViewModel>()
                .ForMember(d => d.StartDate, conf => conf.MapFrom(s => s.StartDate.ToString(CourseConstants.DateFormat)));

            this.CreateMap<Course, CourseSelectionItemViewModel>();
            this.CreateMap<CourseStudentMap, StudentCourseViewModel>()
                .ForMember(d => d.Id, conf => conf.MapFrom(s => s.CourseId))
                .ForMember(d => d.Name, conf => conf.MapFrom(s => s.Course.Name));
        }
    }
}
