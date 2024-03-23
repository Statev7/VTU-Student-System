namespace StudentSystem.Services.Data.Features.Courses.MappingProfiles
{
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Courses.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;
    using StudentSystem.Services.Data.Infrastructure.StaticHelpers;

    public class CourseMappingProfile : BaseMappingProfile
    {
        public CourseMappingProfile()
        {

            this.CreateMap<CourseFormBidningModel, Course>()
                .ForMember(d => d.Description, conf => conf.MapFrom(s => HtmlHelper.Sanitize(s.Description)));

            this.CreateMap<Course, CourseFormBidningModel>();
        }
    }
}
