using System.ComponentModel.DataAnnotations;

namespace QuickTix.API.Entities.DTOs
{
    public class UserLoginDto
    {
        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; init; }
        [Required(ErrorMessage = "Password is reuired")]
        public string Password { get; init; }
    }
}
