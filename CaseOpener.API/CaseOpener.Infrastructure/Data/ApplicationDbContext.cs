using CaseOpener.Infrastructure.Data.Configurations;
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
        public DbSet<InventoryItem> InventoryItems { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CaseOpeningConfiguration());
            modelBuilder.ApplyConfiguration(new DailyRewardConfiguration());
            modelBuilder.ApplyConfiguration(new InventoryItemConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new UserRoleConfiguration());

            base.OnModelCreating(modelBuilder);
        }
    }
}
