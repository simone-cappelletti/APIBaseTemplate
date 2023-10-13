using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Datamodel.Mappers;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Utils;

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
        PagedResult<Airline> Get(SearchAirlineRequest request);

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

        /// <summary>
        /// Available sorting parameters
        /// </summary>
        /// <returns></returns>
        string[] GetSortingParameters();
    }

    /// <summary>
    /// Business class about <see cref="Airline"/> entity
    /// </summary>
    public class AirlineBusiness : IAirlineBusiness
    {
        private readonly ILogger<AirlineBusiness> _logger;
        private readonly IUnitOfWorkFactory _uof;
        private readonly IAirlineRepository _airlineRepository;
        private readonly IRegionRepository _regionRepository;

        protected readonly static OrderByFilter<Datamodel.DbEntities.Airline> OrderByFilter = new OrderByFilter<Datamodel.DbEntities.Airline>()
            .Add(nameof(Datamodel.DbEntities.Airline.AirlineId), i => i.AirlineId)
            .Add(nameof(Datamodel.DbEntities.Airline.Code), i => i.Code)
            .Add(nameof(Datamodel.DbEntities.Airline.Name), i => i.Name);

        public AirlineBusiness(
            ILogger<AirlineBusiness> logger,
            IUnitOfWorkFactory uof,
            IAirlineRepository airlineRepository,
            IRegionRepository regionRepository)
        {
            _logger = logger;
            _uof = uof;
            _airlineRepository = airlineRepository;
            _regionRepository = regionRepository;
        }

        /// <inheritdoc/>
        public Airline Create(Airline airline)
        {
            Verify.IsNot.Null(airline);

            _logger.LogTrace($"{nameof(Airline)}.{nameof(Create)}({airline})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_airlineRepository, _regionRepository).InTransaction())
                {
                    AirlineBusinessHelper.SanitizeNormalize(airline);
                    AirlineBusinessHelper.AirlineCommonValidation(airline, insertMode: true, _airlineRepository);

                    // Mapping new DTO to new db item
                    var newDbItem = Mappers.Airline.ToDb(airline);

                    // Check region
                    var region = _regionRepository.Single(
                        x => x.RegionId == airline.RegionId,
                        ioEx => throw new RegionSingleException(airline.RegionId));
                    newDbItem.Region = region;

                    _airlineRepository.Add(newDbItem);

                    unit.SaveChanges();

                    var result = Mappers.Airline.ToDto(newDbItem);

                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Created airline {newDbItem.AirlineId} with name {newDbItem.Code}", newDbItem.AirlineId, newDbItem.Code);

                    return result;
                }
            }
            catch (BaseException) { throw; }
            catch (Exception ex)
            {
                throw new AirlineException($"Unexpected error in {nameof(Create)}({airline}): {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public void Delete(int airlineId)
        {
            Verify.Is.Positive(airlineId, nameof(airlineId));

            _logger.LogTrace($"{nameof(AirlineBusiness)}.{nameof(Delete)}({airlineId})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_airlineRepository).InTransaction())
                {
                    // Find the db item to delete
                    var entityToDelete = _airlineRepository.Single(x =>
                        x.AirlineId == airlineId,
                        e => new AirlineSingleException(airlineId));

                    // Deleting the item
                    _airlineRepository.Delete(entityToDelete);

                    unit.SaveChanges();
                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Deleted Airline item with id {airlineId}", airlineId);
                }
            }
            catch (AirlineSingleException) { throw; }
            catch (AirlineDeleteException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(airlineId), airlineId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new AirlineDeleteException(airlineId, ex);
            }
        }

        /// <inheritdoc/>
        public PagedResult<Airline> Get(SearchAirlineRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            _logger.LogTrace($"{nameof(AirlineBusiness)}.{nameof(Get)}({request})");

            try
            {
                var result = new PagedResult<Airline>();

                using (var unit = _uof.Get().BoundTo(_airlineRepository).InTransaction())
                {
                    var query = _airlineRepository.Get(request);

                    // OCP - Ordering, counting and pagination
                    query = query.OrderBy(
                        OrderByFilter,
                        request.Sortings,
                        defaultOrderBy: q => q.OrderBy(x => x.AirlineId));
                    result.Sortings = request.Sortings;

                    // Total count
                    result.TotalRecordsWithoutPagination = query.Count();

                    // Pagination
                    query = query.ApplyPagination(request);
                    result.CurrentPageSize = request.PageSize;
                    result.CurrentPageIndex = request.PageIndex;

                    // Query execution
                    var qResult = query.ToList();
                    result.Items = qResult.Select(x => Mappers.Airline.ToDto(x)).ToList();
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
        public Airline GetById(int airlineId)
        {
            Verify.Is.Positive(airlineId, nameof(airlineId));

            _logger.LogTrace($"{nameof(AirlineBusiness)}.{nameof(GetById)}({airlineId})");

            try
            {
                var result = new Airline();
                var request = new SearchAirlineRequest()
                {
                    Filters = new SearchAirlineFilters()
                    {
                        AirlineId = airlineId
                    }
                };

                using (var unit = _uof.Get().BoundTo(_airlineRepository).InTransaction())
                {
                    // Retrive entity
                    var query = _airlineRepository.Get(request);
                    var entity = query.SingleOrDefault() ?? throw new AirlineSingleException(airlineId);

                    result = Mappers.Airline.ToDto(entity);
                }

                return result;
            }
            catch (AirlineSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(airlineId), airlineId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new AirlineException(
                    $"Unexpected error in {nameof(GetById)}: {ex.Message}",
                    ex,
                    AirlineErrorCodes.UNEXPECTED,
                    (nameof(airlineId), airlineId, Visibility.Private));
            }
        }

        /// <inheritdoc/>
        public string[] GetSortingParameters()
            => OrderByFilter.GetOrderByKeys();

        /// <inheritdoc/>
        public Airline Save(Airline airline)
        {
            Verify.IsNot.Null(airline, nameof(airline));

            _logger.LogTrace($"{nameof(AirlineBusiness)}.{nameof(Save)}({airline})");

            if (airline.AirlineId.HasValue)
            {
                return Update(airline);
            }

            return Create(airline);
        }

        /// <inheritdoc/>
        public Airline Update(Airline airline)
        {
            Verify.IsNot.Null(airline);

            _logger.LogTrace($"{nameof(AirlineBusiness)}.{nameof(Update)}({airline})");

            try
            {
                var result = new Airline();

                using (var unit = _uof.Get().BoundTo(_airlineRepository, _regionRepository).InTransaction())
                {
                    AirlineBusinessHelper.SanitizeNormalize(airline);
                    AirlineBusinessHelper.AirlineCommonValidation(airline, insertMode: false, _airlineRepository);

                    // Retrieve db item to update
                    var airlineToUpdate = _airlineRepository.Single(
                        x => x.AirlineId == airline.AirlineId.Value,
                        ioEx => throw new AirlineSingleException(airline.AirlineId.Value));

                    // Update item
                    Mappers.Airline.ToDb(airline, airlineToUpdate);

                    // Check region
                    var region = _regionRepository.Single(
                            x => x.RegionId == airlineToUpdate.RegionId,
                            ioEx => throw new RegionSingleException(airlineToUpdate.RegionId));
                    airlineToUpdate.Region = region;

                    _airlineRepository.Update(airlineToUpdate);

                    unit.SaveChanges();
                    result = Mappers.Airline.ToDto(airlineToUpdate);
                    unit.CompleteTransactionScope();

                    _logger.LogTrace("Updated Airline item with id {airlineId}", airlineToUpdate.AirlineId);
                }

                return result;
            }
            catch (AirlineSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(airline), airline);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new AirlineException(
                    $"Unexpected error in {nameof(Update)}: {ex.Message}",
                    ex,
                    AirlineErrorCodes.UNEXPECTED,
                    (nameof(airline), airline, Visibility.Private));
            }
        }
    }
}
