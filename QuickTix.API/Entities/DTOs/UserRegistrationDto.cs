using System.ComponentModel.DataAnnotations;

namespace QuickTix.API.Entities.DTOs
{
    public class UserRegistrationDto
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; init; }
        [Compare("Password")]
        public required string ConfirmPassword { get; init; }
        public required string PhoneNumber { get; init; }
    }
}
