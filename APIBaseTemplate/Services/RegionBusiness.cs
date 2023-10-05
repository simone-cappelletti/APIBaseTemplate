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
    /// Business interface class about <see cref="Region"/> entity
    /// </summary>
    public interface IRegionBusiness
    {
        /// <summary>
        /// Retrieve <see cref="Region"/> based on <paramref name="request"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PagedResult<Region> Get(SearchRegionRequest request);

        /// <summary>
        /// Retrieve specific <see cref="Region"/> by <paramref name="regionId"/>
        /// </summary>
        /// <param name="regionId"></param>
        /// <returns></returns>
        Region GetById(int regionId);

        /// <summary>
        /// Save a <see cref="Region"/>.
        /// If <see cref="Region.RegionId"/> is null an insert will be performed.
        /// else (if <see cref="Region.RegionId"/> is not null) an update will be performed.
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        Region Save(Region region);

        /// <summary>
        /// Insert a new <see cref="Region"/>
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        Region Create(Region region);

        /// <summary>
        /// Update existing <see cref="Region"/>
        /// </summary>
        /// <param name="region"></param>
        /// <returns></returns>
        Region Update(Region region);

        /// <summary>
        /// Delete an existing <see cref="Region"/>
        /// </summary>
        /// <param name="regionId"></param>
        void Delete(int regionId);

        /// <summary>
        /// Available sorting parameters
        /// </summary>
        /// <returns></returns>
        string[] GetSortingParameters();
    }

    public class RegionBusiness : IRegionBusiness
    {
        private readonly ILogger<RegionBusiness> _logger;
        private readonly IUnitOfWorkFactory _uof;
        private readonly IRegionRepository _regionRepository;

        protected readonly static OrderByFilter<Datamodel.DbEntities.Region> OrderByFilter = new OrderByFilter<Datamodel.DbEntities.Region>()
            .Add("RegionId", i => i.RegionId)
            .Add("Name", i => i.Name);

        public RegionBusiness(
            ILogger<RegionBusiness> logger,
            IUnitOfWorkFactory uof,
            IRegionRepository regionRepository)
        {
            _logger = logger;
            _uof = uof;
            _regionRepository = regionRepository;
        }

        /// <inheritdoc/>
        public Region Create(Region region)
        {
            Verify.IsNot.Null(region);

            _logger.LogTrace($"{nameof(RegionBusiness)}.{nameof(Create)}({region})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_regionRepository).InTransaction())
                {
                    RegionBusinessHelper.SanitizeNormalize(region);
                    RegionBusinessHelper.RegionCommonValidation(region, insertMode: true, _regionRepository);

                    // Mapping new DTO to new db item
                    var newDbItem = Mappers.Region.ToDb(region);

                    _regionRepository.Add(newDbItem);

                    unit.SaveChanges();

                    var result = Mappers.Region.ToDto(newDbItem);

                    unit.CompleteTransactionScope();

                    _logger.LogInformation($"Created region {newDbItem.RegionId} with name {newDbItem.Name}");

                    return result;
                }
            }
            catch (BaseException) { throw; }
            catch (Exception ex)
            {
                throw new RegionException($"Unexpected error in {nameof(Create)}({region}): {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public void Delete(int regionId)
        {
            Verify.Is.Positive(regionId, nameof(regionId));

            _logger.LogTrace($"{nameof(RegionBusiness)}.{nameof(Delete)}({regionId})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_regionRepository).InTransaction())
                {
                    // Find the db item to delete
                    var entityToDelete = _regionRepository.Single(x =>
                        x.RegionId == regionId,
                        e => new RegionSingleException(regionId));

                    // Deleting the item
                    _regionRepository.Delete(entityToDelete);

                    unit.SaveChanges();
                    unit.CompleteTransactionScope();

                    _logger.LogInformation($"Deleted Region item with id {regionId}");
                }
            }
            catch (RegionSingleException) { throw; }
            catch (RegionDeleteException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(regionId), regionId);
                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new RegionDeleteException(regionId, ex);
            }
        }

        /// <inheritdoc/>
        public PagedResult<Region> Get(SearchRegionRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            _logger.LogTrace($"{nameof(RegionBusiness)}.{nameof(Get)}({request})");

            try
            {
                var result = new PagedResult<Region>();

                using (var unit = _uof.Get().BoundTo(_regionRepository))
                {
                    var query = _regionRepository.Get(request);

                    // OCP - Ordering, counting and pagination
                    query = query.OrderBy(
                        OrderByFilter,
                        request.Sortings,
                        defaultOrderBy: q => q.OrderBy(x => x.Name));
                    result.Sortings = request.Sortings;

                    // Total count
                    result.TotalRecordsWithoutPagination = query.Count();

                    // Pagination
                    query = query.ApplyPagination(request);
                    result.CurrentPageSize = request.PageSize;
                    result.CurrentPageIndex = request.PageIndex;

                    // Query execution
                    var qResult = query.ToList();
                    result.Items = qResult.Select(x => Mappers.Region.ToDto(x)).ToList();
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
        public Region GetById(int regionId)
        {
            Verify.Is.Positive(regionId, nameof(regionId));

            _logger.LogTrace($"{nameof(RegionBusiness)}.{nameof(GetById)}({regionId})");

            try
            {
                var result = new Region();
                var request = new SearchRegionRequest()
                {
                    Filters = new SearchRegionFilters()
                    {
                        RegionId = regionId
                    }
                };

                using (var unit = _uof.Get().BoundTo(_regionRepository))
                {
                    // Retrive entity
                    var query = _regionRepository.Get(request);
                    var entity = query.SingleOrDefault() ?? throw new RegionSingleException(regionId);

                    result = Mappers.Region.ToDto(entity);
                }

                return result;
            }
            catch (RegionSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(regionId), regionId);

                throw baseExc;
            }
            catch (Exception ex)
            {
                throw new RegionException(
                    $"Unexpected error in {nameof(GetById)}: {ex.Message}",
                    ex,
                    RegionErrorCodes.UNEXPECTED,
                    (nameof(regionId), regionId, Visibility.Private));
            }
        }

        /// <inheritdoc/>
        public string[] GetSortingParameters()
            => OrderByFilter.GetOrderByKeys();

        /// <inheritdoc/>
        public Region Save(Region region)
        {
            Verify.IsNot.Null(region, nameof(region));

            _logger.LogTrace($"{nameof(RegionBusiness)}.{nameof(Save)}({region})");

            if (region.RegionId.HasValue)
            {
                return Update(region);
            }

            return Create(region);
        }

        /// <inheritdoc/>
        public Region Update(Region region)
        {
            throw new NotImplementedException();
        }
    }
}
