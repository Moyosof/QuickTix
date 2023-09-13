using Microsoft.AspNetCore.Identity;
using QuickTix.API.Entities.DTOs;

namespace QuickTix.API.Repositories.Interfaces
{
    public interface IUserAuth
    {
        Task<string> RegisterUserAsync(UserRegistrationDto userRegistration);
        Task<bool> ValidateUserAsync(UserLoginDto loginDto);
        Task<string> CreateTokenAsync();

    }
}
