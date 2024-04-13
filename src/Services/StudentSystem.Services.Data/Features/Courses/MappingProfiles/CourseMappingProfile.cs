namespace StudentSystem.Services.Data.Features.Courses.MappingProfiles
{
    using StudentSystem.Data.Models.Courses;
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
                .ForMember(d => d.Description, conf => conf.MapFrom(s => HtmlHelper.Sanitize(s.Description)));

            this.CreateMap<Course, CourseFormBindingModel>();
            this.CreateMap<Course, CourseViewModel>()
                .ForMember(d => d.StartDate, conf => conf.MapFrom(s => s.StartDate.ToString("dd MMMM yyyy")))
                .ForMember(d => d.Duration, conf => conf.MapFrom(s => (int)Math.Ceiling((s.EndDate - s.StartDate).TotalDays / 7)))
                .ForMember(d => d.ImageUrl, conf => conf.MapFrom(s => s.ImageFolder));

            this.CreateMap<Course, CoursePaymentDetailsServiceModel>()
                .ForMember(d => d.ImageUrl, conf => conf.MapFrom(s => s.ImageFolder));
        }
    }
}
