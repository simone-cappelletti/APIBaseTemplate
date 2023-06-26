namespace APIBaseTemplate.Repositories.UnitOfWork
{
    /// <summary>
    /// Default implementation of factory per unit of work.
    /// </summary>
    public class UnitOfWorkFactory : IUnitOfWorkFactory
    {
        private readonly Func<IUnitOfWork> _getDefaultUoWFunc;
        private readonly Func<string, IUnitOfWork>? _getNamedUoWFunc;

        /// <summary>
        /// Initialize <see cref="UnitOfWorkFactory"/> 
        /// </summary>
        /// <param name="defaultFactory"></param>
        /// <param name="namedFactory"></param>
        public UnitOfWorkFactory(
            Func<IUnitOfWork> defaultFactory,
            Func<string, IUnitOfWork>? namedFactory = null)
        {
            _getDefaultUoWFunc = defaultFactory;
            _getNamedUoWFunc = namedFactory;
        }

        /// <summary>
        /// Returns (default) <see cref="IUnitOfWork"/> instance
        /// </summary>
        /// <returns></returns>
        public IUnitOfWork Get()
        {
            var result = _getDefaultUoWFunc();
            return result;
        }

        /// <summary>
        /// Returns a specific instance of the work unit.
        /// </summary>
        /// <param name="name"></param>
        /// <returns>Instance of specific IUnitOfWork</returns>
        public IUnitOfWork Get(string name)
        {
            var result = _getNamedUoWFunc(name);
            return result;
        }
    }
}
