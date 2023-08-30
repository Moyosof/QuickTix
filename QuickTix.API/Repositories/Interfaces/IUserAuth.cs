using Microsoft.AspNetCore.Identity;
using QuickTix.API.Entities.DTOs;

namespace QuickTix.API.Repositories.Interfaces
{
    public interface IUserAuth
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto userLoginDto);
        Task<string> CreateTokenAsync();

    }
}
