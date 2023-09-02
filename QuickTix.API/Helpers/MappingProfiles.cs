using QuickTix.API.Entities.DTOs;
using AutoMapper;
using QuickTix.API.Data;

namespace QuickTix.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // All Mapping Configuration Here
            CreateMap<UserRegistrationDto, ApplicationUser>();
        }
    }
}
