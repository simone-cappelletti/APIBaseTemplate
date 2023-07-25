using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;

namespace APIBaseTemplate.Services
{
    /// <summary>
    /// Business interface class about <see cref="Airline"/> entity
    /// </summary>
    public interface IAirlineBusiness
    {
        /// <summary>
        /// Retrieve <see cref="Airline"/> based on <paramref name="request"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<Airline> Get(SearchAirlineRequest request);

        /// <summary>
        /// Retrieve specific <see cref="Airline"/> by <paramref name="airlineId"/>
        /// </summary>
        /// <param name="airlineId"></param>
        /// <returns></returns>
        Airline GetById(int airlineId);

        /// <summary>
        /// Save a <see cref="Airline"/>.
        /// If <see cref="Airline.AirlineId"/> is null an insert will be performed.
        /// else (if <see cref="Airline.AirlineId"/> is not null) an update will be performed.
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        Airline Save(Airline airline);

        /// <summary>
        /// Insert a new <see cref="Airline"/>
        /// </summary>
        /// <param name="airline"></param>
        /// <returns></returns>
        Airline Create(Airline airline);

        /// <summary>
        /// Update existing <see cref="Airline"/>
        /// </summary>
        /// <param name="airline"></param>
        /// <returns></returns>
        Airline Update(Airline airline);

        /// <summary>
        /// Delete an existing <see cref="Airline"/>
        /// </summary>
        /// <param name="airlineId"></param>
        void Delete(int airlineId);
    }

    /// <summary>
    /// Business class about <see cref="Airline"/> entity
    /// </summary>
    public class AirlineBusiness : IAirlineBusiness
    {
        private readonly ILogger<AirlineBusiness> _logger;
        private readonly IUnitOfWorkFactory _uof;
        private readonly IAirlineRepository _airlineRepository;

        public AirlineBusiness(
            ILogger<AirlineBusiness> logger,
            IUnitOfWorkFactory uof,
            IAirlineRepository airlineRepository)
        {
            _logger = logger;
            _uof = uof;
            _airlineRepository = airlineRepository;
        }

        public Airline Create(Airline airline)
        {
            _logger.LogTrace($"{nameof(AirlineBusiness)}.{nameof(Create)}({airline})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_airlineRepository).InTransaction())
                {
                    // to do
                }
            }
            catch (Exception ex)
            {
                // to do
            }

            return null;
        }

        public void Delete(int airlineId)
        {
            throw new NotImplementedException();
        }

        public List<Airline> Get(SearchAirlineRequest request)
        {
            throw new NotImplementedException();
        }

        public Airline GetById(int airlineId)
        {
            throw new NotImplementedException();
        }

        public Airline Save(Airline airline)
        {
            throw new NotImplementedException();
        }

        public Airline Update(Airline airline)
        {
            throw new NotImplementedException();
        }
    }
}
