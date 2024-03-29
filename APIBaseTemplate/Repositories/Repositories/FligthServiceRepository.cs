﻿using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Datamodel.Mappers;
using APIBaseTemplate.Utils;
using FligthService = APIBaseTemplate.Datamodel.DbEntities.FligthService;

namespace APIBaseTemplate.Repositories
{
    public interface IFligthServiceRepository : IRepository<FligthService>
    {
        /// <summary>
        /// It performs a search of <see cref="FligthService"/> items using <paramref name="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        IQueryable<FligthService> Get(SearchFligthServiceRequest filter);

        /// <summary>
        /// It deletes a <see cref="FligthService"/> by id.
        /// </summary>
        /// <param name="fligthServiceId"></param>
        public void DeleteById(int fligthServiceId);
    }

    public class FligthServiceRepository : BaseRepository<FligthService>, IFligthServiceRepository
    {
        private readonly ILogger<FligthServiceRepository> _logger;

        public FligthServiceRepository(ILogger<FligthServiceRepository> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public IQueryable<FligthService> Get(SearchFligthServiceRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            IQueryable<FligthService> query = this.DbContext.Set<FligthService>();

            query = AddIncludes(query, request.Options);
            query = ApplyFilters(query, request.Filters);

            return query;
        }

        /// <inheritdoc/>
        public void DeleteById(int fligthServiceId)
        {
            // Check input
            Verify.Is.Positive(fligthServiceId, nameof(fligthServiceId));

            // Retrieve entity
            var entityToDelete = this.Single(
                i => i.FligthServiceId == fligthServiceId,
                ioEx => throw new FligthServiceSingleException(fligthServiceId));

            _logger.LogInformation($"{nameof(FligthService.FligthServiceId)} deleted {fligthServiceId}");

            Delete(entityToDelete);
        }

        private IQueryable<FligthService> ApplyFilters(IQueryable<FligthService> query, SearchFligthServiceFilters filters)
        {
            if (filters == null)
            {
                return query;
            }

            // FligthServiceId
            if (true == filters.FligthServiceId.HasValue)
            {
                query = query.Where(x => x.FligthServiceId == filters.FligthServiceId);
            }

            // FlightServiceType
            if (filters.FlightServiceType != null && filters.FlightServiceType.Any())
            {
                var dbTypes = Mappers.FligthService.ToDb(filters.FlightServiceType).ToList();
                query = query.Where(x => dbTypes.Contains(x.FlightServiceType));
            }

            // CurrencyId
            if (true == filters.CurrencyId.HasValue)
            {
                query = query.Where(x => x.CurrencyId == filters.CurrencyId);
            }

            // FligthId
            if (true == filters.FligthId.HasValue)
            {
                query = query.Where(x => x.FligthId == filters.FligthId);
            }

            // IdList
            if (filters.IdList != null && !filters.IdList.IsEmpty())
            {
                var idList = filters.IdList.GetArrayIds();
                query = query.Where(x => idList.Contains(x.FligthServiceId));
            }

            // IdRange
            if (filters.IdRange != null && filters.IdRange.IsValidRange())
            {
                query = query.Where(x => x.FligthServiceId >= filters.IdRange.MinId && x.FligthServiceId <= filters.IdRange.MaxId);
            }

            return query;
        }

        private IQueryable<FligthService> AddIncludes(IQueryable<FligthService> query, SearchFligthServiceOptions options)
        {
            return query;
        }
    }
}
