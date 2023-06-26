using APIBaseTemplate.Repositories.DataContexts;

namespace APIBaseTemplate.Repositories.Repositories
{
    public interface IRepository<TEntity> : IDataContextRepository where TEntity : class
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Query();
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="item"></param>
        void Add(TEntity item);
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="item"></param>
        void Update(TEntity item);
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="item"></param>
        void Delete(TEntity item);
    }
}
