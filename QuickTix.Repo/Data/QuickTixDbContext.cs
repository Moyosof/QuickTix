using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickTix.Repo.Data
{
    public class QuickTixDbContext : DbContext
    {
        public QuickTixDbContext(DbContextOptions<QuickTixDbContext> options) : base(options)
        {
            
        }
    }
}
