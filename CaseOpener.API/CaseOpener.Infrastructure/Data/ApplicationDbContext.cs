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
        public DbSet<Item> Items { get; set; }
        public DbSet<Case> Cases { get; set; }
        public DbSet<Transaction> Transactions { get; set; }
        public DbSet<CaseOpening> CaseOpenings { get; set; }
        public DbSet<DailyReward> DailyRewards { get; set; }
    }
}
