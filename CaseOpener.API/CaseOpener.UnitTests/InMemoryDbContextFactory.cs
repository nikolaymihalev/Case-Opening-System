using CaseOpener.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseOpener.UnitTests
{
    public static class InMemoryDbContextFactory
    {
        public static ApplicationDbContext Create()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString()) 
                .Options;

            return new ApplicationDbContext(options);
        }
    }

}
