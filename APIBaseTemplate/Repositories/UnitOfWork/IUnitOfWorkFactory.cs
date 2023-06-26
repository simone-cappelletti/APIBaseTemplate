namespace APIBaseTemplate.Repositories.UnitOfWork
{
    /// <summary>
    /// Factory for the creation of units of work
    /// </summary>
    public interface IUnitOfWorkFactory
    {
        /// <summary>
        /// Returns an instance of IUnitOfWork using the default connection.
        /// </summary>
        /// <returns>Default instance of IUnitOfWork.</returns>
        IUnitOfWork Get();

        /// <summary>
        /// Returns an instance of IUnitOfWork using the connection identified by the string given in the parameter.
        /// </summary>
        /// <param name="connectionIdentifier">ID of the connection to be used to create the specific unit of work.</param>
        /// <returns>Instance of specific IUnitOfWork</returns>
        IUnitOfWork Get(string connectionIdentifier);
    }
}
