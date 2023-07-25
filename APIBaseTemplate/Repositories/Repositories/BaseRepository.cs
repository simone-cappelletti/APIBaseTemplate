using APIBaseTemplate.Repositories.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace APIBaseTemplate.Repositories
{
    /// <summary>
    /// Non-typed Repository
    /// </summary>
    public abstract class BaseRepository : IDataContextRepository
    {
        public IDataContext DataContext { get; set; }

        public DbContext DbContext { get => ((DataContext)DataContext).DbContext; }
    }

    /// <summary>
    /// Typed Repository
    /// </summary>
    public class BaseRepository<TEntity> : BaseRepository, IRepository<TEntity> where TEntity : class
    {
        /// <inheritdoc/>
        public IQueryable<TEntity> Query()
        {
            return DataContext.Query<TEntity>();
        }

        /// <inheritdoc/>
        public void Add(TEntity item)
        {
            DataContext.Add(item);
        }

        /// <inheritdoc/>
        public void Delete(TEntity item)
        {
            DataContext.Delete(item);
        }

        /// <inheritdoc/>
        public void Update(TEntity item)
        {
            DataContext.Update(item);
        }
    }
}
