namespace APIBaseTemplate.Repositories.DataContexts
{
    /// <summary>
    /// Interface that abstracts a connection to the DB via an ORM.
    /// </summary>
    public interface IDataContext
    {
        /// <summary>
        /// Query
        /// </summary>
        /// <returns></returns>
        IQueryable<TEntity> Query<TEntity>() where TEntity : class;

        /// <summary>
        /// Insert
        /// </summary>
        void Add<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Update
        /// </summary>
        void Update<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Delete
        /// </summary>
        void Delete<TEntity>(TEntity item) where TEntity : class;

        /// <summary>
        /// Save changes
        /// </summary>
        int SaveChanges();

        /// <summary>
        /// Clear change tracker
        /// </summary>
        void ClearCache();
    }
}
