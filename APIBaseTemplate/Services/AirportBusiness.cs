using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Repositories;

namespace APIBaseTemplate.Services
{
    /// <summary>
    /// Business interface class about <see cref="Airport"/> entity
    /// </summary>
    public interface IAirportBusiness
    {
        /// <summary>
        /// Retrieve <see cref="Airport"/> based on <paramref name="request"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        List<Airport> Get(SearchAirportRequest request);

        /// <summary>
        /// Retrieve specific <see cref="Airport"/> by <paramref name="airportId"/>
        /// </summary>
        /// <param name="airportId"></param>
        /// <returns></returns>
        Airport GetById(int airportId);

        /// <summary>
        /// Save a <see cref="Airport"/>.
        /// If <see cref="Airport.AirportId"/> is null an insert will be performed.
        /// else (if <see cref="Airport.AirportId"/> is not null) an update will be performed.
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        Airport Save(Airport airport);

        /// <summary>
        /// Insert a new <see cref="Airport"/>
        /// </summary>
        /// <param name="airport"></param>
        /// <returns></returns>
        Airport Create(Airport airport);

        /// <summary>
        /// Update existing <see cref="Airport"/>
        /// </summary>
        /// <param name="airport"></param>
        /// <returns></returns>
        Airport Update(Airport airport);

        /// <summary>
        /// Delete an existing <see cref="Airport"/>
        /// </summary>
        /// <param name="airportId"></param>
        void Delete(int airportId);
    }

    /// <summary>
    /// Business class about <see cref="Airport"/> entity
    /// </summary>
    public class AirportBusiness : IAirportBusiness
    {
        private readonly ILogger<AirportBusiness> _logger;
        private readonly IUnitOfWorkFactory _uof;
        private readonly IAirportRepository _airportRepository;

        public AirportBusiness(
            ILogger<AirportBusiness> logger,
            IUnitOfWorkFactory uof,
            IAirportRepository airportRepository)
        {
            _logger = logger;
            _uof = uof;
            _airportRepository = airportRepository;
        }

        public Airport Create(Airport airport)
        {
            _logger.LogTrace($"{nameof(AirportBusiness)}.{nameof(Create)}({airport})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_airportRepository).InTransaction())
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
