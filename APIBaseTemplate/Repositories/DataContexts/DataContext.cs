using Microsoft.EntityFrameworkCore;

namespace APIBaseTemplate.Repositories.DataContexts
{
    public class DataContext : IDataContext
    {
        /// <summary>
        /// Entity Framework data context.
        /// It can be used to perform some low-level operations
        /// </summary>
        public DbContext DbContext { get; init; }

        /// <summary>
        /// Default constructor to use to have a DataContext.
        /// </summary>
        /// <param name="dbContext">Entity Framework DB Context</param>
        public DataContext(DbContext dbContext)
        {
            DbContext = dbContext;
        }

        /// <inheritdoc/>
        public IQueryable<TEntity> Query<TEntity>() where TEntity : class
        {
            return DbContext.Set<TEntity>();
        }

        /// <inheritdoc/>
        public void Add<TEntity>(TEntity item) where TEntity : class
        {
            DbContext.Add(item);
        }

        /// <inheritdoc/>
        public void Update<TEntity>(TEntity item) where TEntity : class
        {
            DbContext.Update(item);
        }

        /// <inheritdoc/>
        public void Delete<TEntity>(TEntity item) where TEntity : class
        {
            DbContext.Remove(item);
        }

        /// <inheritdoc/>
        public int SaveChanges()
        {
            return DbContext.SaveChanges();
        }

        /// <inheritdoc/>
        public void ClearCache()
            => DbContext.ChangeTracker.Clear();
    }
}
