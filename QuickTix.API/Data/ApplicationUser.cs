using HouseMate.API.Entities.Enums;
using Microsoft.AspNetCore.Identity;

namespace HouseMate.API.Data
{
    public class ApplicationUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public ServicePerference ServicePerferences { get; set; }

    }
}
