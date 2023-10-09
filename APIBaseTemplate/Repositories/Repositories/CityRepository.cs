using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Utils;
using Microsoft.EntityFrameworkCore;
using City = APIBaseTemplate.Datamodel.DbEntities.City;

namespace APIBaseTemplate.Repositories.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        /// <summary>
        /// It performs a search of <see cref="City"/> items using <paramref name="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        IQueryable<City> Get(SearchCityRequest filter);

        /// <summary>
        /// It deletes a <see cref="City"/> by id.
        /// </summary>
        /// <param name="cityId"></param>
        public void DeleteById(int cityId);
    }

    public class CityRepository : BaseRepository<City>, ICityRepository
    {
        private readonly ILogger<CityRepository> _logger;

        public CityRepository(ILogger<CityRepository> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public IQueryable<City> Get(SearchCityRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            IQueryable<City> query = this.DbContext.Set<City>();

            query = AddIncludes(query, request.Options);
            query = ApplyFilters(query, request.Filters);

            return query;
        }

        /// <inheritdoc/>
        public void DeleteById(int cityId)
        {
            // Check input
            Verify.Is.Positive(cityId, nameof(cityId));

            // Retrieve entity
            var entityToDelete = this.Single(
                i => i.CityId == cityId,
                ioEx => throw new CitySingleException(cityId));

            _logger.LogInformation($"CityId deleted {cityId}");

            Delete(entityToDelete);
        }

        private IQueryable<City> ApplyFilters(IQueryable<City> query, SearchCityFilters filters)
        {
            if (filters == null)
            {
                return query;
            }

            var sanitizeOptions = EnmSimpleTextFilterSanitize.RemovePercent | EnmSimpleTextFilterSanitize.Trim | EnmSimpleTextFilterSanitize.ToUpper;

            // CityId
            if (true == filters.CityId.HasValue)
            {
                query = query.Where(x => x.CityId == filters.CityId);
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

            // RegionId
            if (true == filters.RegionId.HasValue)
            {
                query = query.Where(x => x.RegionId == filters.RegionId);
            }

            return query;
        }

        private IQueryable<City> AddIncludes(IQueryable<City> query, SearchCityOptions options)
        {
            query = query.Include(x => x.Region);

            return query;
        }
    }
}
