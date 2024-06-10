namespace StudentSystem.Services.Data.Features.Resources.MappingProfiles
{
    using StudentSystem.Data.Models.Courses;
    using StudentSystem.Services.Data.Features.Resources.DTOs.BindingModels;
    using StudentSystem.Services.Data.Features.Resources.DTOs.ServiceModels;
    using StudentSystem.Services.Data.Features.Resources.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    public class ResourceMappingProfile : BaseMappingProfile
    {
        public ResourceMappingProfile()
        {
            this.CreateMap<ResourceBindingModel, Resource>().ReverseMap();

            this.CreateMap<Resource, ResourceViewModel>();

            this.CreateMap<Resource, ResourceDetailsServiceModel>();
        }
    }
}
