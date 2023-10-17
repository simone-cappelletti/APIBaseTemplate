using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Utils;
using Microsoft.EntityFrameworkCore;
using Region = APIBaseTemplate.Datamodel.DbEntities.Region;

namespace APIBaseTemplate.Repositories.Repositories
{
    public interface IRegionRepository : IRepository<Region>
    {
        /// <summary>
        /// It performs a search of <see cref="Region"/> items using <paramref name="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        IQueryable<Region> Get(SearchRegionRequest filter);

        /// <summary>
        /// It deletes a <see cref="Region"/> by id.
        /// </summary>
        /// <param name="regionId"></param>
        public void DeleteById(int regionId);
    }

    public class RegionRepository : BaseRepository<Region>, IRegionRepository
    {
        private readonly ILogger<RegionRepository> _logger;

        public RegionRepository(ILogger<RegionRepository> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public IQueryable<Region> Get(SearchRegionRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            IQueryable<Region> query = this.DbContext.Set<Region>();

            query = AddIncludes(query, request.Options);
            query = ApplyFilters(query, request.Filters);

            return query;
        }

        /// <inheritdoc/>
        public void DeleteById(int regionId)
        {
            // Check input
            Verify.Is.Positive(regionId, nameof(regionId));

            // Retrieve entity
            var entityToDelete = this.Single(
                i => i.RegionId == regionId,
                ioEx => throw new RegionSingleException(regionId));

            _logger.LogInformation($"{nameof(Region.RegionId)} deleted {regionId}");

            Delete(entityToDelete);
        }

        private IQueryable<Region> ApplyFilters(IQueryable<Region> query, SearchRegionFilters filters)
        {
            if (filters == null)
            {
                return query;
            }

            var sanitizeOptions = EnmSimpleTextFilterSanitize.RemovePercent | EnmSimpleTextFilterSanitize.Trim | EnmSimpleTextFilterSanitize.ToUpper;

            // RegionId
            if (true == filters.RegionId.HasValue)
            {
                query = query.Where(x => x.RegionId == filters.RegionId);
            }

            // Name
            if (filters.Name != null)
            {
                filters.Name.Validate();
                string text = filters.Name.GetSimpleSanitizedValue(sanitizeOptions);
                query = query.WhereTextFilter(filters.Name.Operator,
                    isNull: null,
                    equalTo: x => x.Name.ToUpper() == text,
                    like: x => EF.Functions.Like(x.Name.ToUpper(), $"%{text}%"),
                    startsWith: x => EF.Functions.Like(x.Name.ToUpper(), $"{text}%"),
                    endsWith: x => EF.Functions.Like(x.Name.ToUpper(), $"%{text}"),
                    lessThan: x => x.Name.ToUpper().CompareTo(text) < 0,
                    greaterThan: x => x.Name.ToUpper().CompareTo(text) > 0,
                    inValues: null);
            }

            return query;
        }

        private IQueryable<Region> AddIncludes(IQueryable<Region> query, SearchRegionOptions options)
        {
            return query;
        }
    }
}
