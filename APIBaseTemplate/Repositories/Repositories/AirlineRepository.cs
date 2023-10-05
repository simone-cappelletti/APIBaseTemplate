using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Utils;
using Microsoft.EntityFrameworkCore;
using Airline = APIBaseTemplate.Datamodel.DbEntities.Airline;

namespace APIBaseTemplate.Repositories
{
    public interface IAirlineRepository : IRepository<Airline>
    {
        /// <summary>
        /// It performs a search of <see cref="Airline"/> items using <paramref name="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        IQueryable<Airline> Get(SearchAirlineRequest filter);

        /// <summary>
        /// It deletes a <see cref="Airline"/> by id.
        /// </summary>
        /// <param name="airlineId"></param>
        public void DeleteById(int airlineId);
    }

    public class AirlineRepository : BaseRepository<Airline>, IAirlineRepository
    {
        private readonly ILogger<AirlineRepository> _logger;

        public AirlineRepository(ILogger<AirlineRepository> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public IQueryable<Airline> Get(SearchAirlineRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            IQueryable<Airline> query = this.DbContext.Set<Airline>();

            query = ApplyFilters(query, request.Filters);
            query = AddIncludes(query, request.Options);

            return query;
        }

        /// <inheritdoc/>
        public void DeleteById(int airlineId)
        {
            // Check input
            Verify.Is.Positive(airlineId, nameof(airlineId));

            // Retrieve entity
            var entityToDelete = this.Single(
                i => i.AirlineId == airlineId,
                ioEx => throw new AirlineSingleException(airlineId));

            _logger.LogInformation($"AirlineId deleted {airlineId}");

            Delete(entityToDelete);
        }

        private IQueryable<Airline> ApplyFilters(IQueryable<Airline> query, SearchAirlineFilters filters)
        {
            if (filters == null)
                return query;

            var sanitizeOptions = EnmSimpleTextFilterSanitize.RemovePercent | EnmSimpleTextFilterSanitize.Trim | EnmSimpleTextFilterSanitize.ToUpper;

            // AirlineId
            if (true == filters.AirlineId.HasValue)
            {
                query = query.Where(x => x.AirlineId == filters.AirlineId);
            }

            // Code
            if (filters.Code != null)
            {
                filters.Code.Validate();
                string text = filters.Code.GetSimpleSanitizedValue(sanitizeOptions);
                query = query.WhereTextFilter(filters.Code.Operator,
                    isNull: null,
                    equalTo: x => x.Code.ToUpper() == text,
                    like: x => EF.Functions.Like(x.Code.ToUpper(), $"%{text}%"),
                    startsWith: x => EF.Functions.Like(x.Code.ToUpper(), $"{text}%"),
                    endsWith: x => EF.Functions.Like(x.Code.ToUpper(), $"%{text}"),
                    lessThan: x => x.Code.ToUpper().CompareTo(text) < 0,
                    greaterThan: x => x.Code.ToUpper().CompareTo(text) > 0,
                    inValues: null);
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
            if (false == filters.RegionId.HasValue)
            {
                query = query.Where(x => x.RegionId == filters.RegionId);
            }

            return query;
        }

        private IQueryable<Airline> AddIncludes(IQueryable<Airline> query, SearchAirlineOptions options)
        {
            if (options == null)
                return query;

            query = query.Include(x => x.Region);

            return query;
        }
    }
}
