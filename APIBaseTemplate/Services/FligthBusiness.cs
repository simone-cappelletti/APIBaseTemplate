using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Datamodel.Mappers;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Services
{
    public interface IFligthBusiness
    {
        /// <summary>
        /// Retrieve <see cref="Fligth"/> based on <paramref name="request"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PagedResult<Fligth> Get(SearchFligthRequest request);

        /// <summary>
        /// Retrieve specific <see cref="Fligth"/> by <paramref name="fligthId"/>
        /// </summary>
        /// <param name="fligthId"></param>
        /// <returns></returns>
        Fligth GetById(int fligthId);

        /// <summary>
        /// Save a <see cref="Fligth"/>.
        /// If <see cref="Fligth.FligthId"/> is null an insert will be performed.
        /// else (if <see cref="Fligth.FligthId"/> is not null) an update will be performed.
        /// </summary>
        /// <param name="stock"></param>
        /// <returns></returns>
        Fligth Save(Fligth fligth);

        /// <summary>
        /// Insert a new <see cref="Fligth"/>
        /// </summary>
        /// <param name="fligth"></param>
        /// <returns></returns>
        Fligth Create(Fligth fligth);

        /// <summary>
        /// Update existing <see cref="Fligth"/>
        /// </summary>
        /// <param name="fligth"></param>
        /// <returns></returns>
        Fligth Update(Fligth fligth);

        /// <summary>
        /// Delete an existing <see cref="Fligth"/>
        /// </summary>
        /// <param name="fligthId"></param>
        void Delete(int fligthId);

        /// <summary>
        /// Available sorting parameters
        /// </summary>
        /// <returns></returns>
        string[] GetSortingParameters();
    }

    /// <summary>
    /// Business class about <see cref="Fligth"/> entity
    /// </summary>
    public class FligthBusiness : IFligthBusiness
    {
        private readonly ILogger<FligthBusiness> _logger;
        private readonly IUnitOfWorkFactory _uof;
        private readonly IFligthRepository _fligthRepository;
        private readonly IAirlineRepository _airlineRepository;
        private readonly IAirportRepository _airportRepository;
        private readonly IFligthServiceRepository _fligthServiceRepository;

        protected readonly static OrderByFilter<Datamodel.DbEntities.Fligth> OrderByFilter = new OrderByFilter<Datamodel.DbEntities.Fligth>()
            .Add(nameof(Datamodel.DbEntities.Fligth.FligthId), i => i.FligthId)
            .Add(nameof(Datamodel.DbEntities.Fligth.Code), i => i.Code);

        public FligthBusiness(
            ILogger<FligthBusiness> logger,
            IUnitOfWorkFactory uof,
            IFligthRepository fligthRepository,
            IAirlineRepository airlineRepository,
            IAirportRepository airportRepository,
            IFligthServiceRepository fligthServiceRepository)
        {
            _logger = logger;
            _uof = uof;
            _fligthRepository = fligthRepository;
            _airlineRepository = airlineRepository;
            _airportRepository = airportRepository;
            _fligthServiceRepository = fligthServiceRepository;
        }

        /// <inheritdoc/>
        public Fligth Create(Fligth fligth)
        {
            Verify.IsNot.Null(fligth);

            _logger.LogTrace($"{nameof(Fligth)}.{nameof(Create)}({fligth})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_fligthRepository, _airlineRepository, _airportRepository).InTransaction())
                {
                    FligthBusinessHelper.SanitizeNormalize(fligth);
                    FligthBusinessHelper.FligthCommonValidation(fligth, insertMode: true, _fligthRepository);

                    // Mapping new DTO to new db item
                    var newDbItem = Mappers.Fligth.ToDb(fligth);

                    // Check Airline
                    var airline = _airlineRepository.Single(
                        x => x.AirlineId == fligth.AirlineId,
                        ioEx => throw new AirlineSingleException(fligth.AirlineId));
                    newDbItem.Airline = airline;

                    // Check DepartureAirport
                    var departureAirport = _airportRepository.Single(
                        x => x.AirportId == fligth.DepartureAirportId,
                        ioEx => throw new AirportSingleException(fligth.DepartureAirportId));
                    newDbItem.DepartureAirport = departureAirport;

                    // Check ArrivalAirport
                    var arrivalAirport = _airportRepository.Single(
                        x => x.AirportId == fligth.ArrivalAirportId,
                        ioEx => throw new AirportSingleException(fligth.ArrivalAirportId));
                    newDbItem.ArrivalAirport = arrivalAirport;

                    // Check FligthServices
                    var fligthServices = new List<Datamodel.DbEntities.FligthService>();
                    foreach (var fligthService in fligth.FligthServices)
                    {
                        fligthServices.Add(
                                _fligthServiceRepository.Single(
                            x => x.FligthServiceId == fligthService.FligthServiceId,
                            ioEx => throw new FligthServiceSingleException(fligth.ArrivalAirportId))
                            );
                    }
                    newDbItem.FligthServices = fligthServices;

                    _fligthRepository.Add(newDbItem);

                    unit.SaveChanges();

                    var result = Mappers.Fligth.ToDto(newDbItem);

                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Created fligth {newDbItem.FligthId} with name {newDbItem.Code}", newDbItem.FligthId, newDbItem.Code);

                    return result;
                }
            }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(fligth), fligth);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new FligthException($"Unexpected error in {nameof(Create)}({fligth}): {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public void Delete(int fligthId)
        {
            Verify.Is.Positive(fligthId, nameof(fligthId));

            _logger.LogTrace($"{nameof(FligthBusiness)}.{nameof(Delete)}({fligthId})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_fligthRepository).InTransaction())
                {
                    // Find the db item to delete
                    var entityToDelete = _fligthRepository.Single(x =>
                        x.FligthId == fligthId,
                        e => new FligthSingleException(fligthId));

                    // Deleting the item
                    _fligthRepository.Delete(entityToDelete);

                    unit.SaveChanges();
                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Deleted Fligth item with id {fligthId}", fligthId);
                }
            }
            catch (FligthSingleException) { throw; }
            catch (FligthDeleteException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(fligthId), fligthId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new FligthDeleteException(fligthId, ex);
            }
        }

        /// <inheritdoc/>
        public PagedResult<Fligth> Get(SearchFligthRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            _logger.LogTrace($"{nameof(FligthBusiness)}.{nameof(Get)}({request})");

            try
            {
                var result = new PagedResult<Fligth>();

                using (var unit = _uof.Get().BoundTo(_fligthRepository).InTransaction())
                {
                    var query = _fligthRepository.Get(request);

                    // OCP - Ordering, counting and pagination
                    query = query.OrderBy(
                        OrderByFilter,
                        request.Sortings,
                        defaultOrderBy: q => q.OrderBy(x => x.FligthId));
                    result.Sortings = request.Sortings;

                    // Total count
                    result.TotalRecordsWithoutPagination = query.Count();

                    // Pagination
                    query = query.ApplyPagination(request);
                    result.CurrentPageSize = request.PageSize;
                    result.CurrentPageIndex = request.PageIndex;

                    // Query execution
                    var qResult = query.ToList();
                    result.Items = qResult.Select(x => Mappers.Fligth.ToDto(x)).ToList();
                }

                return result;
            }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(request), request);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new BaseException($"Unexpected error in {nameof(Get)}: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public Fligth GetById(int fligthId)
        {
            Verify.Is.Positive(fligthId, nameof(fligthId));

            _logger.LogTrace($"{nameof(FligthBusiness)}.{nameof(GetById)}({fligthId})");

            try
            {
                var result = new Fligth();
                var request = new SearchFligthRequest()
                {
                    Filters = new SearchFligthFilters()
                    {
                        FligthId = fligthId
                    }
                };

                using (var unit = _uof.Get().BoundTo(_fligthRepository).InTransaction())
                {
                    // Retrive entity
                    var query = _fligthRepository.Get(request);
                    var entity = query.SingleOrDefault() ?? throw new FligthSingleException(fligthId);

                    result = Mappers.Fligth.ToDto(entity);
                }

                return result;
            }
            catch (FligthSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(fligthId), fligthId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new FligthException(
                    $"Unexpected error in {nameof(GetById)}: {ex.Message}",
                    ex,
                    FligthErrorCodes.UNEXPECTED,
                    (nameof(fligthId), fligthId, Visibility.Private));
            }
        }

        /// <inheritdoc/>
        public string[] GetSortingParameters()
            => OrderByFilter.GetOrderByKeys();

        /// <inheritdoc/>
        public Fligth Save(Fligth Fligtf)
        {
            Verify.IsNot.Null(Fligtf, nameof(Fligtf));

            _logger.LogTrace($"{nameof(FligthBusiness)}.{nameof(Save)}({Fligtf})");

            if (Fligtf.FligthId.HasValue)
            {
                return Update(Fligtf);
            }

            return Create(Fligtf);
        }

        /// <inheritdoc/>
        public Fligth Update(Fligth fligth)
        {
            Verify.IsNot.Null(fligth);

            _logger.LogTrace($"{nameof(FligthBusiness)}.{nameof(Update)}({fligth})");

            try
            {
                var result = new Fligth();

                using (var unit = _uof.Get().BoundTo(_fligthRepository, _airlineRepository, _airportRepository).InTransaction())
                {
                    FligthBusinessHelper.SanitizeNormalize(fligth);
                    FligthBusinessHelper.FligthCommonValidation(fligth, insertMode: false, _fligthRepository);

                    // Retrieve db item to update
                    var fligthToUpdate = _fligthRepository.Single(
                        x => x.FligthId == fligth.FligthId.Value,
                        ioEx => throw new FligthSingleException(fligth.FligthId.Value));

                    // Update item
                    Mappers.Fligth.ToDb(fligth, fligthToUpdate);

                    // Check Airline
                    var airline = _airlineRepository.Single(
                        x => x.AirlineId == fligth.AirlineId,
                        ioEx => throw new AirlineSingleException(fligth.AirlineId));
                    fligthToUpdate.Airline = airline;

                    // Check DepartureAirport
                    var departureAirport = _airportRepository.Single(
                        x => x.AirportId == fligth.DepartureAirportId,
                        ioEx => throw new AirportSingleException(fligth.DepartureAirportId));
                    fligthToUpdate.DepartureAirport = departureAirport;

                    // Check ArrivalAirport
                    var arrivalAirport = _airportRepository.Single(
                        x => x.AirportId == fligth.ArrivalAirportId,
                        ioEx => throw new AirportSingleException(fligth.ArrivalAirportId));
                    fligthToUpdate.ArrivalAirport = arrivalAirport;

                    // Check FligthServices
                    var fligthServices = new List<Datamodel.DbEntities.FligthService>();
                    foreach (var fligthService in fligth.FligthServices)
                    {
                        fligthServices.Add(
                                _fligthServiceRepository.Single(
                            x => x.FligthServiceId == fligthService.FligthServiceId,
                            ioEx => throw new FligthServiceSingleException(fligthService.FligthServiceId.Value))
                            );
                    }
                    fligthToUpdate.FligthServices = fligthServices;

                    _fligthRepository.Update(fligthToUpdate);

                    unit.SaveChanges();
                    result = Mappers.Fligth.ToDto(fligthToUpdate);
                    unit.CompleteTransactionScope();

                    _logger.LogTrace("Updated Fligth item with id {fligthId}", fligthToUpdate.FligthId);
                }

                return result;
            }
            catch (FligthSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(fligth), fligth);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new FligthException(
                    $"Unexpected error in {nameof(Update)}: {ex.Message}",
                    ex,
                    FligthErrorCodes.UNEXPECTED,
                    (nameof(fligth), fligth, Visibility.Private));
            }
        }
    }
}
