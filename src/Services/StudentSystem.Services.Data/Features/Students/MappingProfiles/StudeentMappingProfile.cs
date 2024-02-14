namespace StudentSystem.Services.Data.Features.Students.MappingProfiles
{
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Students.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    public class StudeentMappingProfile : BaseMappingProfile
    {
        public StudeentMappingProfile()
        {
            this.CreateMap<BecomeStudentBindingModel, Student>();

            this.CreateMap<Student, PendingStudentViewModel>()
                .ForMember(d => d.FullName, conf => conf.MapFrom(s => $"{s.FirstName} {s.LastName}"))
                .ForMember(d => d.CityName, conf => conf.MapFrom(s => s.City.Name))
                .ForMember(d => d.Email, conf => conf.MapFrom(s => s.User.Email))
                .ForMember(d => d.AppliedOn, conf => conf.MapFrom(s => s.CreatedOn));
        }
    }
}
