using HouseMate.API.Entities.Enums;
using System.ComponentModel.DataAnnotations;

namespace HouseMate.API.Entities.DTOs
{
    public class UserRegistrationDto
    {
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Email { get; init; }
        public required string Username { get; init; }

        [Required(ErrorMessage = "Password is required")]
        public required string Password { get; init; }
        [Compare("Password")]
        public required string ConfirmPassword { get; init; }
        public required string PhoneNumber { get; init; }
        public required string State { get; set; }
        public required string City { get; set; }
        public required ServicePerference ServicePerferences { get; set; }
        //public required ServicePerference ServicePerferences { get; set; }
    }
}
