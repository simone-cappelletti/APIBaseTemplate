using APIBaseTemplate.Datamodel.DbEntities;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.DataContexts;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace APIBaseTemplateUnitTests
{
    public class MockData
    {
        public Mock<IDataContext> DataContext { get; private set; } = new Mock<IDataContext>();
        public Mock<IDataContextRepository> DataContextRepository { get; private set; } = new Mock<IDataContextRepository>();
        public Mock<IUnitOfWork> UnitOfWork { get; private set; } = new Mock<IUnitOfWork>();
        public Mock<IUnitOfWorkFactory> UnitOfWorkFactory { get; private set; } = new Mock<IUnitOfWorkFactory>();

        #region Repository
        public Mock<IRepository<Currency>> CurrencyRepository { get; private set; } = new Mock<IRepository<Currency>>();
        public Mock<ICityRepository> CityRepository { get; private set; } = new Mock<ICityRepository>();
        public Mock<IRegionRepository> RegionRepository { get; private set; } = new Mock<IRegionRepository>();
        public Mock<IAirlineRepository> AirlineRepository { get; private set; } = new Mock<IAirlineRepository>();
        public Mock<IAirportRepository> AirportRepository { get; private set; } = new Mock<IAirportRepository>();
        public Mock<IFligthRepository> FligthRepository { get; private set; } = new Mock<IFligthRepository>();
        public Mock<IFligthServiceRepository> FligthServiceRepository { get; private set; } = new Mock<IFligthServiceRepository>();

        private readonly Dictionary<Type, object> _repositories = new Dictionary<Type, object>();
        public Mock<IRepository<T>> Repository<T>() where T : class
        {
            var type = typeof(T);

            if (!_repositories.ContainsKey(type))
            {
                _repositories[type] = new Mock<IRepository<T>>();
            }

            object result = _repositories[type];

            return (Mock<IRepository<T>>)result;
        }
        #endregion

        #region Business
        public Mock<ICityBusiness> CityBusiness { get; private set; } = new Mock<ICityBusiness>();
        public Mock<IRegionBusiness> RegionBusiness { get; private set; } = new Mock<IRegionBusiness>();
        public Mock<IAirlineBusiness> AirlineBusiness { get; private set; } = new Mock<IAirlineBusiness>();
        public Mock<IAirportBusiness> AirportBusiness { get; private set; } = new Mock<IAirportBusiness>();
        public Mock<IFligthBusiness> FligthBusiness { get; private set; } = new Mock<IFligthBusiness>();
        public Mock<IFligthServiceBusiness> FligthServiceBusiness { get; private set; } = new Mock<IFligthServiceBusiness>();
        #endregion

        private readonly Dictionary<Type, object> _loggers = new Dictionary<Type, object>();

        public Mock<ILogger<T>> Logger<T>()
        {
            var type = typeof(T);

            if (!_loggers.ContainsKey(type))
            {
                _loggers[type] = new Mock<ILogger<T>>();
            }

            object result = _loggers[type];

            return (Mock<ILogger<T>>)result;
        }

        public MockData()
        {
            UnitOfWork
                .Setup(x => x.BoundTo(It.IsAny<IDataContextRepository[]>()))
                .Returns(UnitOfWork.Object);

            UnitOfWork
                .Setup(x => x.InTransaction()
                ).Returns(UnitOfWork.Object);

            UnitOfWorkFactory
                .Setup(x => x.Get())
                .Returns(UnitOfWork.Object);
        }
    }
}
