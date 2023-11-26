using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using HouseMate.API.Entities;

namespace HouseMate.API.Data
{
    public class HouseMateDbContext : IdentityDbContext<ApplicationUser>
    {
 

        public HouseMateDbContext(DbContextOptions<HouseMateDbContext> options) : base(options)
        {
            
        }
    }
}
