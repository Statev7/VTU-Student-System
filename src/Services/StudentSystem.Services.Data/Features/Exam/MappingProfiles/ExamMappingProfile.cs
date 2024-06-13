namespace StudentSystem.Services.Data.Features.Exam.MappingProfiles
{
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Exam.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure.StaticHelpers;

    public class ExamMappingProfile : BaseMappingProfile
    {
        public ExamMappingProfile()
        {
            this.CreateMap<Exam, ExamBindingModel>()
                .ForMember(d => d.Comment, conf => conf.MapFrom(s => HtmlHelper.Sanitize(s.Comment)));

            this.CreateMap<ExamBindingModel, Exam>();
        }
    }
}
