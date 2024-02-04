namespace StudentSystem.Services.Data.Features.City.MappingProfiles
{
    using StudentSystem.Data.Models.Users;
    using StudentSystem.Services.Data.Abstaction.Mapper;
    using StudentSystem.Services.Data.Features.City.DTOs.ViewModels;

    public class CityMappingProfile : BaseMappingProfile
    {
        public CityMappingProfile()
        {
            CreateMap<City, CityViewModel>().ReverseMap();
        }
    }
}
