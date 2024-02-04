namespace StudentSystem.Services.Data.Features.Students.MappingProfiles
{
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Abstaction.Mapper;
    using StudentSystem.Services.Data.Features.Students.DTOs.BindingModels;

    public class StudeentMappingProfile : BaseMappingProfile
    {
        public StudeentMappingProfile()
        {
            this.CreateMap<BecomeStudentBindingModel, Student>();
        }
    }
}
