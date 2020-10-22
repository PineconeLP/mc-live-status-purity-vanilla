using AutoMapper;
using MCLiveStatus.Domain.Models;
using MCLiveStatus.EntityFramework.Models;

namespace MCLiveStatus.EntityFramework.Mappers
{
    public class DTOMappingProfile : Profile
    {
        public DTOMappingProfile()
        {
            CreateMap<ServerPingerSettingsDTO, ServerPingerSettings>().ReverseMap();
        }
    }
}