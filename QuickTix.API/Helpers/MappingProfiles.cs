using QuickTix.API.Entities.DTOs;
using QuickTix.API.Entities;
using AutoMapper;

namespace QuickTix.API.Helpers
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            // Provide Mapping Configurations Her
            CreateMap<UserRegistrationDto, ApplicationUser>();
        }
    }
}
