namespace StudentSystem.Services.Data.Features.Lessons.MappingProfiles
{
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Lessons.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;
    using StudentSystem.Services.Data.Infrastructure.StaticHelpers;

    public class LessonMappingProfile : BaseMappingProfile
    {
        public LessonMappingProfile()
        {
            this.CreateMap<LessonFormBindingModel, Lesson>()
                .ForMember(d => d.Description, conf => conf.MapFrom(s => HtmlHelper.Sanitize(s.Description)));

            this.CreateMap<Lesson, LessonScheduleViewModel>()
                .ForMember(d => d.CourseName, conf => conf.MapFrom(s => s.Course.Name));
        }
    }
}
