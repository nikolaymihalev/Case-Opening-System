using CaseOpener.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace CaseOpener.Infrastructure.Common
{
    public class Repository : IRepository
    {
        public DbContext Context;

        private DbSet<T> DbSet<T>() where T : class => this.Context.Set<T>();

        public Repository(ApplicationDbContext _context)
        {
            Context = _context;
        }

        public async Task AddAsync<T>(T entity) where T : class => await this.DbSet<T>().AddAsync(entity);

        public IQueryable<T> All<T>() where T : class => this.DbSet<T>().AsQueryable();

        public IQueryable<T> AllReadonly<T>() where T : class => this.DbSet<T>().AsQueryable().AsNoTracking();

        public async Task DeleteAsync<T>(object id) where T : class
        {
            T? entity = await GetByIdAsync<T>(id);

            if (entity != null)
            {
                this.DbSet<T>().Remove(entity);
            }
        }

        public async Task<T?> GetByIdAsync<T>(object id) where T : class => await this.DbSet<T>().FindAsync(id);

        public async Task<int> SaveChangesAsync() => await this.Context.SaveChangesAsync();
    }
}
