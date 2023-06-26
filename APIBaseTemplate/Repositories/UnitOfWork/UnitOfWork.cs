using APIBaseTemplate.Repositories.DataContexts;
using System.Transactions;

namespace APIBaseTemplate.Repositories.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // The datacontext
        private readonly IDataContext _dataContext;
        // Repository list
        private readonly List<IDataContextRepository> _boundRepositories = new ();

        private TransactionScope _scope;

        /// <summary>
        /// Transaction scope
        /// </summary>
        public TransactionScope Scope { get => _scope; }

        /// <summary>
        /// Crea una Unit Of Work EF usando una connessione tramite Data Context
        /// </summary>
        /// <param name="dataContext"></param>
        public UnitOfWork(IDataContext dataContext)
        {
            _dataContext = dataContext;
        }

        public IUnitOfWork BoundTo(params IDataContextRepository[] respositories)
        {
            foreach (var repository in respositories)
            {
                repository.DataContext = _dataContext;
                _boundRepositories.Add(repository);
            }

            return this;
        }

        public void CompleteTransactionScope() 
            => _scope.Complete();

        public void Dispose() 
            => _scope.Dispose();

        public IUnitOfWork InTransaction()
        {
            _scope = _scope ?? new TransactionScope();
            return this;
        }

        public int SaveChanges() 
            => _dataContext.SaveChanges();
    }
}
