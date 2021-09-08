using AutoMapper;
using PlatformService.Dtos;
using PlatformService.Models;

namespace PlatformService.Profiles
{
    public class PlatoformsProfile : Profile
    {
        public PlatoformsProfile()
        {
            // Source -> Target
            CreateMap<Platform,PlatformReadDto>();
            CreateMap<PlatformCreateDto, Platform>();
        }
    }
}