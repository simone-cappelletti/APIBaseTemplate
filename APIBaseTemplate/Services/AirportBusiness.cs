using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Common.Exceptions.Airport;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Datamodel.Mappers;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Utils;

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
        private readonly ICityRepository _cityRepository;

        protected readonly static OrderByFilter<Datamodel.DbEntities.Airport> OrderByFilter = new OrderByFilter<Datamodel.DbEntities.Airport>()
            .Add(nameof(Datamodel.DbEntities.Airport.AirportId), i => i.AirportId)
            .Add(nameof(Datamodel.DbEntities.Airport.Code), i => i.Code)
            .Add(nameof(Datamodel.DbEntities.Airport.Name), i => i.Name);

        public AirportBusiness(
            ILogger<AirportBusiness> logger,
            IUnitOfWorkFactory uof,
            IAirportRepository airportRepository,
            ICityRepository cityRepository)
        {
            _logger = logger;
            _uof = uof;
            _airportRepository = airportRepository;
            _cityRepository = cityRepository;
        }

        /// <inheritdoc/>
        public Airport Create(Airport airport)
        {
            Verify.IsNot.Null(airport);

            _logger.LogTrace($"{nameof(Airport)}.{nameof(Create)}({airport})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_airportRepository, _cityRepository).InTransaction())
                {
                    AirportBusinessHelper.SanitizeNormalize(airport);
                    AirportBusinessHelper.AirportCommonValidation(airport, insertMode: true, _airportRepository);

                    // Mapping new DTO to new db item
                    var newDbItem = Mappers.Airport.ToDb(airport);

                    // Check city
                    var city = _cityRepository.Single(
                    x => x.CityId == airport.CityId,
                        ioEx => throw new CitySingleException(airport.CityId));
                    newDbItem.City = city;

                    _airportRepository.Add(newDbItem);

                    unit.SaveChanges();

                    var result = Mappers.Airport.ToDto(newDbItem);

                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Created Airport {newDbItem.AirportId} with name {newDbItem.Code}", newDbItem.AirportId, newDbItem.Code);

                    return result;
                }
            }
            catch (BaseException) { throw; }
            catch (Exception ex)
            {
                throw new AirportException($"Unexpected error in {nameof(Create)}({airport}): {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public void Delete(int airportId)
        {
            Verify.Is.Positive(airportId, nameof(airportId));

            _logger.LogTrace($"{nameof(AirportBusiness)}.{nameof(Delete)}({airportId})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_airportRepository).InTransaction())
                {
                    // Find the db item to delete
                    var entityToDelete = _airportRepository.Single(x =>
                        x.AirportId == airportId,
                        e => new AirportSingleException(airportId));

                    // Deleting the item
                    _airportRepository.Delete(entityToDelete);

                    unit.SaveChanges();
                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Deleted Airport item with id {airportId}", airportId);
                }
            }
            catch (AirportSingleException) { throw; }
            catch (AirportDeleteException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(airportId), airportId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new AirportDeleteException(airportId, ex);
            }
        }

        /// <inheritdoc/>
        public PagedResult<Airport> Get(SearchAirportRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            _logger.LogTrace($"{nameof(AirportBusiness)}.{nameof(Get)}({request})");

            try
            {
                var result = new PagedResult<Airport>();

                using (var unit = _uof.Get().BoundTo(_airportRepository).InTransaction())
                {
                    var query = _airportRepository.Get(request);

                    // OCP - Ordering, counting and pagination
                    query = query.OrderBy(
                        OrderByFilter,
                        request.Sortings,
                        defaultOrderBy: q => q.OrderBy(x => x.AirportId));
                    result.Sortings = request.Sortings;

                    // Total count
                    result.TotalRecordsWithoutPagination = query.Count();

                    // Pagination
                    query = query.ApplyPagination(request);
                    result.CurrentPageSize = request.PageSize;
                    result.CurrentPageIndex = request.PageIndex;

                    // Query execution
                    var qResult = query.ToList();
                    result.Items = qResult.Select(x => Mappers.Airport.ToDto(x)).ToList();
                }

                return result;
            }
            catch (BaseException) { throw; }
            catch (Exception ex)
            {
                throw new BaseException($"Unexpected error in {nameof(Get)}: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public Airport GetById(int airportId)
        {
            Verify.Is.Positive(airportId, nameof(airportId));

            _logger.LogTrace($"{nameof(AirportBusiness)}.{nameof(GetById)}({airportId})");

            try
            {
                var result = new Airport();
                var request = new SearchAirportRequest()
                {
                    Filters = new SearchAirportFilters()
                    {
                        AirportId = airportId
                    }
                };

                using (var unit = _uof.Get().BoundTo(_airportRepository).InTransaction())
                {
                    // Retrive entity
                    var query = _airportRepository.Get(request);
                    var entity = query.SingleOrDefault() ?? throw new AirportSingleException(airportId);

                    result = Mappers.Airport.ToDto(entity);
                }

                return result;
            }
            catch (AirportSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(airportId), airportId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new AirportException(
                    $"Unexpected error in {nameof(GetById)}: {ex.Message}",
                    ex,
                    AirportErrorCodes.UNEXPECTED,
                    (nameof(airportId), airportId, Visibility.Private));
            }
        }

        /// <inheritdoc/>
        public string[] GetSortingParameters()
            => OrderByFilter.GetOrderByKeys();

        /// <inheritdoc/>
        public Airport Save(Airport airport)
        {
            Verify.IsNot.Null(airport, nameof(airport));

            _logger.LogTrace($"{nameof(AirportBusiness)}.{nameof(Save)}({airport})");

            if (airport.AirportId.HasValue)
            {
                return Update(airport);
            }

            return Create(airport);
        }

        /// <inheritdoc/>
        public Airport Update(Airport airport)
        {
            Verify.IsNot.Null(airport);

            _logger.LogTrace($"{nameof(AirportBusiness)}.{nameof(Update)}({airport})");

            try
            {
                var result = new Airport();

                using (var unit = _uof.Get().BoundTo(_airportRepository, _cityRepository).InTransaction())
                {
                    AirportBusinessHelper.SanitizeNormalize(airport);
                    AirportBusinessHelper.AirportCommonValidation(airport, insertMode: false, _airportRepository);

                    // Retrieve db item to update
                    var airportToUpdate = _airportRepository.Single(
                        x => x.AirportId == airport.AirportId.Value,
                        ioEx => throw new AirportSingleException(airport.AirportId.Value));

                    // Update item
                    Mappers.Airport.ToDb(airport, airportToUpdate);

                    // Check city
                    var city = _cityRepository.Single(
                            x => x.CityId == airportToUpdate.CityId,
                            ioEx => throw new CitySingleException(airportToUpdate.CityId));
                    airportToUpdate.City = city;

                    _airportRepository.Update(airportToUpdate);

                    unit.SaveChanges();
                    result = Mappers.Airport.ToDto(airportToUpdate);
                    unit.CompleteTransactionScope();

                    _logger.LogTrace("Updated Airport item with id {airlineId}", airportToUpdate.AirportId);
                }

                return result;
            }
            catch (AirportSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(airport), airport);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new AirportException(
                    $"Unexpected error in {nameof(Update)}: {ex.Message}",
                    ex,
                    AirportErrorCodes.UNEXPECTED,
                    (nameof(airport), airport, Visibility.Private));
            }
        }
    }
}
