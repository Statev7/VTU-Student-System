namespace StudentSystem.Services.Data.Features.Teachers.MappingProfiles
{
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    public class TeacherMappingProfile : BaseMappingProfile
    {
        public TeacherMappingProfile()
        {
            this.CreateMap<Teacher, TeacherSelectionItemViewModel>()
                .ForMember(d => d.FullName, conf => conf.MapFrom(s => $"{s.User.FirstName} {s.User.LastName}"));
        }
    }
}
