using APIBaseTemplate.Repositories.DataContexts;
using System.Transactions;

namespace APIBaseTemplate.Repositories.UnitOfWork
{
    /// <summary>
    /// Unit of Work
    /// </summary>
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Transaction scope being used.
        /// </summary>
        public TransactionScope Scope { get; }

        /// <summary>
        /// Creates a TransactionScope that places the unit of work in the transaction.
        /// </summary>
        /// <returns>Returns the object itself, so that commands can be queued on creation.</returns>
        IUnitOfWork InTransaction();

        /// <summary>
        /// Set the data connection to the specified repository list.
        /// </summary>
        /// <param name="respositories">List of repositories you want to use in the unit of work.</param>
        /// <returns>Returns the object itself, so that commands can be queued on creation.</returns>
        IUnitOfWork BoundTo(params IDataContextRepository[] respositories);

        /// <summary>
        /// Performs the commit of the TansactionScope set, if any.
        /// </summary>
        void CompleteTransactionScope();

        /// <summary>
        /// Query flush
        /// </summary>
        int SaveChanges();
    }
}
