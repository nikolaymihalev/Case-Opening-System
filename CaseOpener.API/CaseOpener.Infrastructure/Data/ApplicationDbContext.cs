using CaseOpener.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace CaseOpener.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {            
        }

        public DbSet<User> Users { get; set; }
    }
}
