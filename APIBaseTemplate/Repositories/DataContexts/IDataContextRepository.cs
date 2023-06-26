namespace APIBaseTemplate.Repositories.DataContexts
{
    /// <summary>
    /// Basic interface for a repository.
    /// </summary>
    public interface IDataContextRepository
    {
        /// <summary>
        /// Data source used by the repository to perform queries.
        /// </summary>
        IDataContext DataContext { get; set; }
    }
}
