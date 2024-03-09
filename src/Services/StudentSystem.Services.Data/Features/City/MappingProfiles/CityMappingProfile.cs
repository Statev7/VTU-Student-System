namespace StudentSystem.Services.Data.Features.City.MappingProfiles
{
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Features.City.DTOs.ViewModels;
    using StudentSystem.Services.Data.Infrastructure.Abstaction.Mapper;

    public class CityMappingProfile : BaseMappingProfile
    {
        public CityMappingProfile()
        {
            CreateMap<City, CityViewModel>().ReverseMap();
        }
    }
}
