using HouseMate.API.Entities.DTOs;
using AutoMapper;
using HouseMate.API.Data;

namespace HouseMate.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // All Mapping Configuration Here
            CreateMap<UserRegistrationDto, ApplicationUser>();
            CreateMap<ApplicationUser, UserDetailsDto>();
        }
    }
}
