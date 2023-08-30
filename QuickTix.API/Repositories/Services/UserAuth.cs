using AutoMapper;
using Microsoft.AspNetCore.Identity;
using QuickTix.API.Entities;
using QuickTix.API.Entities.DTOs;
using QuickTix.API.Repositories.Interfaces;

namespace QuickTix.API.Repositories.Services
{
    public class UserAuth : IUserAuth
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private ApplicationUser _user;

        public UserAuth(UserManager<ApplicationUser> userManager, IMapper mapper, IConfiguration configuration)
        {
            _userManager = userManager;
            _mapper = mapper;
            _configuration = configuration;
        }
        public async Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration)
        {
            var user = _mapper.Map<ApplicationUser>(userRegistration);
            var result = await _userManager.CreateAsync(user, userRegistration.Password);
            return result;
        }
    }
}
