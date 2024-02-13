namespace StudentSystem.Services.Data.Features.Students.MappingProfiles
{
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    public class StudeentMappingProfile : BaseMappingProfile
    {
        public StudeentMappingProfile()
        {
            this.CreateMap<BecomeStudentBindingModel, Student>();
        }
    }
}
