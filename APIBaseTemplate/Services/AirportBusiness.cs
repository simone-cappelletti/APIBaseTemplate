using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Common;

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
        PagedResult<Airport> Get(SearchAirportRequest request);

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

        /// <summary>
        /// Available sorting parameters
        /// </summary>
        /// <returns></returns>
        string[] GetSortingParameters();
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Delete(int airportId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public PagedResult<Airport> Get(SearchAirportRequest request)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Airport GetById(int airportId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public string[] GetSortingParameters()
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Airport Save(Airport airport)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc/>
        public Airport Update(Airport airport)
        {
            throw new NotImplementedException();
        }
    }
}
