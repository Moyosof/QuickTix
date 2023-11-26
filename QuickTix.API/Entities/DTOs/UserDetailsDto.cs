using HouseMate.API.Entities.Enums;
using static HouseMate.API.Data.ApplicationUser;

namespace HouseMate.API.Entities.DTOs
{
    public class UserDetailsDto
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string FirstName {  get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phonenumber { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public ServicePerference ServicePerferences { get; set; }

    }
}
