using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Datamodel.Mappers;
using APIBaseTemplate.Repositories;
using APIBaseTemplate.Repositories.UnitOfWork;
using APIBaseTemplate.Utils;

namespace APIBaseTemplate.Services
{
    /// <summary>
    /// Business interface class about <see cref="FligthService"/> entity
    /// </summary>
    public interface IFligthServiceBusiness
    {
        /// <summary>
        /// Retrieve <see cref="FligthService"/> based on <paramref name="request"/>
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        PagedResult<FligthService> Get(SearchFligthServiceRequest request);

        /// <summary>
        /// Retrieve specific <see cref="FligthService"/> by <paramref name="fligthServiceId"/>
        /// </summary>
        /// <param name="fligthServiceId"></param>
        /// <returns></returns>
        FligthService GetById(int fligthServiceId);

        /// <summary>
        /// Save a <see cref="FligthService"/>.
        /// If <see cref="FligthService.FligthServiceId"/> is null an insert will be performed.
        /// else (if <see cref="FligthService.FligthServiceId"/> is not null) an update will be performed.
        /// </summary>
        /// <param name="fligthService"></param>
        /// <returns></returns>
        FligthService Save(FligthService fligthService);

        /// <summary>
        /// Insert a new <see cref="FligthService"/>
        /// </summary>
        /// <param name="fligthService"></param>
        /// <returns></returns>
        FligthService Create(FligthService fligthService);

        /// <summary>
        /// Update existing <see cref="FligthService"/>
        /// </summary>
        /// <param name="fligthService"></param>
        /// <returns></returns>
        FligthService Update(FligthService fligthService);

        /// <summary>
        /// Delete an existing <see cref="FligthService"/>
        /// </summary>
        /// <param name="fligthServiceId"></param>
        void Delete(int fligthServiceId);

        /// <summary>
        /// Available sorting parameters
        /// </summary>
        /// <returns></returns>
        string[] GetSortingParameters();
    }

    public class FligthServiceBusiness : IFligthServiceBusiness
    {
        private readonly ILogger<FligthServiceBusiness> _logger;
        private readonly IUnitOfWorkFactory _uof;
        private readonly IFligthServiceRepository _fligthServiceRepository;
        private readonly IFligthRepository _fligthRepository;
        private readonly IRepository<Datamodel.DbEntities.Currency> _currencyRepository;

        protected readonly static OrderByFilter<Datamodel.DbEntities.FligthService> OrderByFilter = new OrderByFilter<Datamodel.DbEntities.FligthService>()
            .Add(nameof(Datamodel.DbEntities.FligthService.FligthServiceId), i => i.FligthServiceId)
            .Add(nameof(Datamodel.DbEntities.FligthService.FligthId), i => i.FligthId);

        public FligthServiceBusiness(
            ILogger<FligthServiceBusiness> logger,
            IUnitOfWorkFactory uof,
            IFligthServiceRepository fligthServiceRepository,
            IFligthRepository fligthRepository,
            IRepository<Datamodel.DbEntities.Currency> currencyRepository)
        {
            _logger = logger;
            _uof = uof;
            _fligthServiceRepository = fligthServiceRepository;
            _fligthRepository = fligthRepository;
            _currencyRepository = currencyRepository;
        }

        /// <inheritdoc/>
        public FligthService Create(FligthService fligthService)
        {
            Verify.IsNot.Null(fligthService);

            _logger.LogTrace($"{nameof(FligthServiceBusiness)}.{nameof(Create)}({fligthService})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_fligthServiceRepository, _fligthRepository).InTransaction())
                {
                    FligthServiceBusinessHelper.SanitizeNormalize(fligthService);
                    FligthServiceBusinessHelper.FligthServiceCommonValidation(fligthService, insertMode: true, _fligthServiceRepository);

                    // Mapping new DTO to new db item
                    var newDbItem = Mappers.FligthService.ToDb(fligthService);

                    // Check Currency
                    var currency = _currencyRepository.Single(
                        x => x.CurrencyId == fligthService.CurrencyId,
                        ioEx => throw new CurrencySingleException(fligthService.CurrencyId));
                    newDbItem.Currency = currency;

                    // Check Fligth
                    var fligth = _fligthRepository.Single(
                        x => x.FligthId == fligthService.FligthId,
                        ioEx => throw new FligthServiceSingleException(fligthService.FligthId));
                    newDbItem.Fligth = fligth;

                    _fligthServiceRepository.Add(newDbItem);

                    unit.SaveChanges();

                    var result = Mappers.FligthService.ToDto(newDbItem);

                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Created Fligth Service {newDbItem.FligthServiceId}", newDbItem.FligthServiceId);

                    return result;
                }
            }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(fligthService), fligthService);

                throw;
            }
            catch (Exception ex)
            {
                throw new FligthServiceException($"Unexpected error in {nameof(Create)}({fligthService}): {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public void Delete(int fligthServiceId)
        {
            Verify.Is.Positive(fligthServiceId, nameof(fligthServiceId));

            _logger.LogTrace($"{nameof(FligthServiceBusiness)}.{nameof(Delete)}({fligthServiceId})");

            try
            {
                using (var unit = _uof.Get().BoundTo(_fligthServiceRepository).InTransaction())
                {
                    // Find the db item to delete
                    var entityToDelete = _fligthServiceRepository.Single(x =>
                        x.FligthServiceId == fligthServiceId,
                        e => new FligthServiceSingleException(fligthServiceId));

                    // Deleting the item
                    _fligthServiceRepository.Delete(entityToDelete);

                    unit.SaveChanges();
                    unit.CompleteTransactionScope();

                    _logger.LogInformation("Deleted Fligth Service item with id {fligthServiceId}", fligthServiceId);
                }
            }
            catch (FligthServiceSingleException) { throw; }
            catch (FligthServiceDeleteException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(fligthServiceId), fligthServiceId);

                throw;
            }
            catch (Exception ex)
            {
                throw new FligthServiceDeleteException(fligthServiceId, ex);
            }
        }

        /// <inheritdoc/>
        public PagedResult<FligthService> Get(SearchFligthServiceRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            _logger.LogTrace($"{nameof(FligthServiceBusiness)}.{nameof(Get)}({request})");

            try
            {
                var result = new PagedResult<FligthService>();

                using (var unit = _uof.Get().BoundTo(_fligthServiceRepository).InTransaction())
                {
                    var query = _fligthServiceRepository.Get(request);

                    // OCP - Ordering, counting and pagination
                    query = query.OrderBy(
                        OrderByFilter,
                        request.Sortings,
                        defaultOrderBy: q => q.OrderBy(x => x.FligthServiceId));
                    result.Sortings = request.Sortings;

                    // Total count
                    result.TotalRecordsWithoutPagination = query.Count();

                    // Pagination
                    query = query.ApplyPagination(request);
                    result.CurrentPageSize = request.PageSize;
                    result.CurrentPageIndex = request.PageIndex;

                    // Query execution
                    var qResult = query.ToList();
                    result.Items = qResult.Select(x => Mappers.FligthService.ToDto(x)).ToList();
                }

                return result;
            }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(request), request);

                throw;
            }
            catch (Exception ex)
            {
                throw new BaseException($"Unexpected error in {nameof(Get)}: {ex.Message}", ex);
            }
        }

        /// <inheritdoc/>
        public FligthService GetById(int fligthServiceId)
        {
            Verify.Is.Positive(fligthServiceId, nameof(fligthServiceId));

            _logger.LogTrace($"{nameof(FligthServiceBusiness)}.{nameof(GetById)}({fligthServiceId})");

            try
            {
                var result = new FligthService();
                var request = new SearchFligthServiceRequest()
                {
                    Filters = new SearchFligthServiceFilters()
                    {
                        FligthServiceId = fligthServiceId
                    }
                };

                using (var unit = _uof.Get().BoundTo(_fligthServiceRepository).InTransaction())
                {
                    // Retrive entity
                    var query = _fligthServiceRepository.Get(request);
                    var entity = query.SingleOrDefault() ?? throw new FligthServiceSingleException(fligthServiceId);

                    result = Mappers.FligthService.ToDto(entity);
                }

                return result;
            }
            catch (FligthServiceSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(fligthServiceId), fligthServiceId);

                throw;
            }
            catch (Exception ex)
            {
                throw new FligthServiceException(
                    $"Unexpected error in {nameof(GetById)}: {ex.Message}",
                    ex,
                    FligthServiceErrorCodes.UNEXPECTED,
                    (nameof(fligthServiceId), fligthServiceId, Visibility.Private));
            }
        }

        /// <inheritdoc/>
        public string[] GetSortingParameters()
            => OrderByFilter.GetOrderByKeys();

        /// <inheritdoc/>
        public FligthService Save(FligthService fligthService)
        {
            Verify.IsNot.Null(fligthService, nameof(fligthService));

            _logger.LogTrace($"{nameof(FligthServiceBusiness)}.{nameof(Save)}({fligthService})");

            if (fligthService.FligthServiceId.HasValue)
            {
                return Update(fligthService);
            }

            return Create(fligthService);
        }

        /// <inheritdoc/>
        public FligthService Update(FligthService fligthService)
        {
            Verify.IsNot.Null(fligthService);

            _logger.LogTrace($"{nameof(FligthServiceBusiness)}.{nameof(Update)}({fligthService})");

            try
            {
                var result = new FligthService();

                using (var unit = _uof.Get().BoundTo(_fligthServiceRepository, _fligthRepository).InTransaction())
                {
                    FligthServiceBusinessHelper.SanitizeNormalize(fligthService);
                    FligthServiceBusinessHelper.FligthServiceCommonValidation(fligthService, insertMode: false, _fligthServiceRepository);

                    // Retrieve db item to update
                    var fligthServiceToUpdate = _fligthServiceRepository.Single(
                        x => x.FligthServiceId == fligthService.FligthServiceId.Value,
                        ioEx => throw new FligthServiceSingleException(fligthService.FligthServiceId.Value));

                    // Update item
                    Mappers.FligthService.ToDb(fligthService, fligthServiceToUpdate);

                    _fligthServiceRepository.Update(fligthServiceToUpdate);

                    unit.SaveChanges();
                    result = Mappers.FligthService.ToDto(fligthServiceToUpdate);
                    unit.CompleteTransactionScope();

                    _logger.LogTrace("Updated Fligth Service item with id {fligthServiceId}", fligthServiceToUpdate.FligthServiceId);
                }

                return result;
            }
            catch (FligthServiceSingleException) { throw; }
            catch (BaseException baseExc)
            {
                baseExc.PublicAndPrivateErrorCodeParameters.Add(nameof(fligthService), fligthService);

                throw;
            }
            catch (Exception ex)
            {
                throw new FligthServiceException(
                    $"Unexpected error in {nameof(Update)}: {ex.Message}",
                    ex,
                    FligthServiceErrorCodes.UNEXPECTED,
                    (nameof(fligthService), fligthService, Visibility.Private));
            }
        }
    }
}
