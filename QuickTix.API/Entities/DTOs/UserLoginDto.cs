using System.ComponentModel.DataAnnotations;

namespace HouseMate.API.Entities.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; init; }
        [Required(ErrorMessage = "Password is required")]
        public string Password { get; init; }
    }
}
