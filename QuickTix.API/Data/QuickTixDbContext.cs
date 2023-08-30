using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using QuickTix.API.Entities;

namespace QuickTix.API.Data
{
    public class QuickTixDbContext : IdentityDbContext<ApplicationUser>
    {
 

        public QuickTixDbContext(DbContextOptions<QuickTixDbContext> options) : base(options)
        {
            
        }
    }
}
