using Microsoft.AspNetCore.Identity;
using HouseMate.API.Entities.DTOs;
using HouseMate.API.Entities.Enums;
using HouseMate.API.Data;

namespace HouseMate.API.Repositories.Interfaces
{
    public interface IUserAuth
    {
        Task<IdentityResult> RegisterUserAsync(UserRegistrationDto userRegistration);
        Task<(LoginResult result, string message)> ValidateUserAsync(UserLoginDto loginDto);
        Task<string> CreateTokenAsync(UserLoginDto user);
        Task<UserDetailsDto> GetUserByUsername(string email);
    }
}
