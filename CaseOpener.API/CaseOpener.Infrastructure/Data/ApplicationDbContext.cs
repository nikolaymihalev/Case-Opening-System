using Microsoft.EntityFrameworkCore;

namespace CaseOpener.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options):base(options)
        {            
        }
    }
}
