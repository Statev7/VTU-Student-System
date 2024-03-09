namespace StudentSystem.Services.Data.Features.Users.MappingProfiles
{
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.Teachers.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Users.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    public class UserMappingProfile : BaseMappingProfile
    {
        public UserMappingProfile()
        {
            this.CreateMap<ApplicationUser, UserViewModel>()
                .ForMember(d => d.RolesNames, conf => conf.MapFrom(s => s.UserRoles.Select(x => x.Role.Name)));

            this.CreateMap<ApplicationUser, BecomeTeacherBindingModel>();
        }
    }
}
