using AutoMapper;
using QuickTix.API.Entities;
using QuickTix.API.Entities.DTOs;

namespace QuickTix.API.Mappings
{
    public class UserMappingProfile : Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserRegistrationDto, ApplicationUser>();
        }
    }
}
