using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Datamodel.Mappers;
using APIBaseTemplate.Repositories.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Services
{
    /// <summary>
    /// Business interface class about <see cref="City"/> entity
    /// </summary>
    public interface ICityBusiness
    {
        /// <summary>
        /// Retrieve <see cref="City"/> based on <paramref name="request"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PagedResult<City> Get(SearchCityRequest request);

        /// <summary>
        /// Retrieve specific <see cref="City"/> by <paramref name="cityId"/>
        /// </summary>
        /// <param name="cityId"></param>
        /// <returns></returns>
        City GetById(int cityId);

        /// <summary>
        /// Save a <see cref="City"/>.
        /// If <see cref="City.CityId"/> is null an insert will be performed.
        /// else (if <see cref="City.CityId"/> is not null) an update will be performed.
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        City Save(City city);

        /// <summary>
        /// Insert a new <see cref="City"/>
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        City Create(City city);

        /// <summary>
        /// Update existing <see cref="City"/>
        /// </summary>
        /// <param name="city"></param>
        /// <returns></returns>
        City Update(City city);

        /// <summary>
        /// Delete an existing <see cref="City"/>
        /// </summary>
        /// <param name="cityId"></param>
        void Delete(int cityId);

        /// <summary>
        /// Available sorting parameters
        /// </summary>
        /// <returns></returns>
        string[] GetSortingParameters();
    }

    public class CityBusiness : ICityBusiness
    {
        private readonly ILogger<CityBusiness> _logger;
        private readonly IUnitOfWorkFactory _uof;
        private readonly ICityRepository _cityRepository;

        protected readonly static OrderByFilter<Datamodel.DbEntities.City> OrderByFilter = new OrderByFilter<Datamodel.DbEntities.City>()
            .Add(nameof(Datamodel.DbEntities.City.CityId), i => i.CityId)
            .Add(nameof(Datamodel.DbEntities.City.Name), i => i.Name);

        public CityBusiness(
            ILogger<CityBusiness> logger,
            IUnitOfWorkFactory uof,
            ICityRepository cityRepository)
        {
            _logger = logger;
            _uof = uof;
            _cityRepository = cityRepository;
        }

        /// <inheritdoc/>
        public City Create(City city)
        {
            Verify.IsNot.Null(city);

            _logger.LogTrace($"{nameof(CityBusiness)}.{nameof(Create)}({city})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_cityRepository).InTransaction())
                {
                    CityBusinessHelper.SanitizeNormalize(city);
                    CityBusinessHelper.CityCommonValidation(city, insertMode: true, _cityRepository);

                    // Mapping new DTO to new db item
                    var newDbItem = Mappers.City.ToDb(city);

                    _cityRepository.Add(newDbItem);

                    unit.SaveChanges();

                    var result = Mappers.City.ToDto(newDbItem);

                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Created city {newDbItem.CityId} with name {newDbItem.Name}", newDbItem.CityId, newDbItem.Name);

                    return result;
                }
            }
            catch (BaseException) { throw; }
            catch (Exception ex)
            {
                throw new CityException($"Unexpected error in {nameof(Create)}({city}): {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public void Delete(int cityId)
        {
            Verify.Is.Positive(cityId, nameof(cityId));

            _logger.LogTrace($"{nameof(CityBusiness)}.{nameof(Delete)}({cityId})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_cityRepository).InTransaction())
                {
                    // Find the db item to delete
                    var entityToDelete = _cityRepository.Single(x =>
                        x.CityId == cityId,
                        e => new CitySingleException(cityId));

                    // Deleting the item
                    _cityRepository.Delete(entityToDelete);

                    unit.SaveChanges();
                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Deleted City item with id {cityId}", cityId);
                }
            }
            catch (CitySingleException) { throw; }
            catch (CityDeleteException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(cityId), cityId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new CityDeleteException(cityId, ex);
            }
        }

        /// <inheritdoc/>
        public PagedResult<City> Get(SearchCityRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            _logger.LogTrace($"{nameof(CityBusiness)}.{nameof(Get)}({request})");

            try
            {
                var result = new PagedResult<City>();

                using (var unit = _uof.Get().BoundTo(_cityRepository).InTransaction())
                {
                    var query = _cityRepository.Get(request);

                    // OCP - Ordering, counting and pagination
                    query = query.OrderBy(
                        OrderByFilter,
                        request.Sortings,
                        defaultOrderBy: q => q.OrderBy(x => x.CityId));
                    result.Sortings = request.Sortings;

                    // Total count
                    result.TotalRecordsWithoutPagination = query.Count();

                    // Pagination
                    query = query.ApplyPagination(request);
                    result.CurrentPageSize = request.PageSize;
                    result.CurrentPageIndex = request.PageIndex;

                    // Query execution
                    var qResult = query.ToList();
                    result.Items = qResult.Select(x => Mappers.City.ToDto(x)).ToList();
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
        public City GetById(int cityId)
        {
            Verify.Is.Positive(cityId, nameof(cityId));

            _logger.LogTrace($"{nameof(CityBusiness)}.{nameof(GetById)}({cityId})");

            try
            {
                var result = new City();
                var request = new SearchCityRequest()
                {
                    Filters = new SearchCityFilters()
                    {
                        CityId = cityId
                    }
                };

                using (var unit = _uof.Get().BoundTo(_cityRepository).InTransaction())
                {
                    // Retrive entity
                    var query = _cityRepository.Get(request);
                    var entity = query.SingleOrDefault() ?? throw new CitySingleException(cityId);

                    result = Mappers.City.ToDto(entity);
                }

                return result;
            }
            catch (CitySingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(cityId), cityId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new CityException(
                    $"Unexpected error in {nameof(GetById)}: {ex.Message}",
                    ex,
                    CityErrorCodes.UNEXPECTED,
                    (nameof(cityId), cityId, Visibility.Private));
            }
        }

        /// <inheritdoc/>
        public string[] GetSortingParameters()
            => OrderByFilter.GetOrderByKeys();

        /// <inheritdoc/>
        public City Save(City city)
        {
            Verify.IsNot.Null(city, nameof(city));

            _logger.LogTrace($"{nameof(CityBusiness)}.{nameof(Save)}({city})");

            if (city.CityId.HasValue)
            {
                return Update(city);
            }

            return Create(city);
        }

        /// <inheritdoc/>
        public City Update(City city)
        {
            Verify.IsNot.Null(city);

            _logger.LogTrace($"{nameof(CityBusiness)}.{nameof(Update)}({city})");

            try
            {
                var result = new City();

                using (var unit = _uof.Get().BoundTo(_cityRepository).InTransaction())
                {
                    CityBusinessHelper.SanitizeNormalize(city);
                    CityBusinessHelper.CityCommonValidation(city, insertMode: false, _cityRepository);

                    // Retrieve db item to update
                    var cityToUpdate = _cityRepository.Single(
                        x => x.CityId == city.CityId.Value,
                        ioEx => throw new CitySingleException(city.CityId.Value));

                    // Update item
                    Mappers.City.ToDb(city, cityToUpdate);

                    _cityRepository.Update(cityToUpdate);

                    unit.SaveChanges();
                    result = Mappers.City.ToDto(cityToUpdate);
                    unit.CompleteTransactionScope();

                    _logger.LogTrace("Updated City item with id {cityId}", cityToUpdate.CityId);
                }

                return result;
            }
            catch (CitySingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(city), city);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new CityException(
                    $"Unexpected error in {nameof(Update)}: {ex.Message}",
                    ex,
                    CityErrorCodes.UNEXPECTED,
                    (nameof(city), city, Visibility.Private));
            }
        }
    }
}
