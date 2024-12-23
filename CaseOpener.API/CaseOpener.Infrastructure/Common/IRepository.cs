namespace CaseOpener.Infrastructure.Common
{
    /// <summary>
    /// Interface for repository methods
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// All data in a table
        /// </summary>
        IQueryable<T> All<T>() where T : class;

        /// <summary>
        /// All data as no tracking in a table
        /// </summary>
        IQueryable<T> AllReadonly<T>() where T : class;

        /// <summary>
        /// Add entity to the database
        /// </summary>
        Task AddAsync<T>(T entity) where T : class;

        /// <summary>
        /// Delete entity from database
        /// </summary>
        Task DeleteAsync<T>(object id) where T : class;

        /// <summary>
        /// Get specific entity from database by identifier
        /// </summary>
        Task<T?> GetByIdAsync<T>(object id) where T : class;

        /// <summary>
        /// Save changes in database
        /// </summary>
        Task<int> SaveChangesAsync();
    }
}
