using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Exceptions;
using APIBaseTemplate.Datamodel.DTO;
using APIBaseTemplate.Utils;
using Microsoft.EntityFrameworkCore;
using Fligth = APIBaseTemplate.Datamodel.DbEntities.Fligth;

namespace APIBaseTemplate.Repositories
{
    public interface IFligthRepository : IRepository<Fligth>
    {
        /// <summary>
        /// It performs a search of <see cref="Fligth"/> items using <paramref name="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        IQueryable<Fligth> Get(SearchFligthRequest filter);

        /// <summary>
        /// It deletes a <see cref="Fligth"/> by id.
        /// </summary>
        /// <param name="fligthId"></param>
        public void DeleteById(int fligthId);
    }

    public class FligthRepository : BaseRepository<Fligth>, IFligthRepository
    {
        private readonly ILogger<FligthRepository> _logger;

        public FligthRepository(ILogger<FligthRepository> logger)
        {
            _logger = logger;
        }

        /// <inheritdoc/>
        public IQueryable<Fligth> Get(SearchFligthRequest request)
        {
            Verify.IsNot.Null(request, nameof(request));

            IQueryable<Fligth> query = this.DbContext.Set<Fligth>();

            query = ApplyFilters(query, request.Filters);
            query = AddIncludes(query, request.Options);

            return query;
        }

        /// <inheritdoc/>
        public void DeleteById(int fligthId)
        {
            // Check input
            Verify.Is.Positive(fligthId, nameof(fligthId));

            // Retrieve entity
            var entityToDelete = this.Single(
                i => i.FligthId == fligthId,
                ioEx => throw new FligthSingleException(fligthId));

            _logger.LogInformation($"{nameof(Fligth.FligthId)} deleted {fligthId}");

            Delete(entityToDelete);
        }

        private IQueryable<Fligth> ApplyFilters(IQueryable<Fligth> query, SearchFligthFilters filters)
        {
            if (filters == null)
                return query;

            var sanitizeOptions = EnmSimpleTextFilterSanitize.RemovePercent | EnmSimpleTextFilterSanitize.Trim | EnmSimpleTextFilterSanitize.ToUpper;

            // FligthId
            if (true == filters.FligthId.HasValue)
            {
                query = query.Where(x => x.FligthId == filters.FligthId);
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

            // AirlineId
            if (false == filters.AirlineId.HasValue)
            {
                query = query.Where(x => x.AirlineId == filters.AirlineId);
            }

            // DepartureAirportId
            if (false == filters.DepartureAirportId.HasValue)
            {
                query = query.Where(x => x.DepartureAirportId == filters.DepartureAirportId);
            }

            // ArrivalAirportId
            if (false == filters.ArrivalAirportId.HasValue)
            {
                query = query.Where(x => x.ArrivalAirportId == filters.ArrivalAirportId);
            }

            // DepartureTime
            if (filters.DepartureTime != null)
            {
                query = query.WhereDateTimeFilter(filters.DepartureTime.Operator,
                    isNull: s => s == null,
                    equalTo: s => s.DepartureTime == filters.DepartureTime.Value,
                    lessThan: s => s.DepartureTime < filters.DepartureTime.Value,
                    greaterThan: s => s.DepartureTime > filters.DepartureTime.Value,
                    between: s => (filters.DepartureTime.Value <= s.DepartureTime &&
                        s.DepartureTime <= filters.DepartureTime.Value2)
                    );
            }

            // ArrivalTime
            if (filters.ArrivalTime != null)
            {
                query = query.WhereDateTimeFilter(filters.ArrivalTime.Operator,
                    isNull: s => s == null,
                    equalTo: s => s.ArrivalTime == filters.ArrivalTime.Value,
                    lessThan: s => s.ArrivalTime < filters.ArrivalTime.Value,
                    greaterThan: s => s.ArrivalTime > filters.ArrivalTime.Value,
                    between: s => (filters.ArrivalTime.Value <= s.ArrivalTime &&
                        s.ArrivalTime <= filters.ArrivalTime.Value2)
                    );
            }

            // Terminal
            if (filters.Terminal != null)
            {
                filters.Terminal.Validate();
                string text = filters.Terminal.GetSimpleSanitizedValue(sanitizeOptions);
                query = query.WhereTextFilter(filters.Terminal.Operator,
                    isNull: null,
                    equalTo: x => x.Terminal.ToUpper() == text,
                    like: x => EF.Functions.Like(x.Terminal.ToUpper(), $"%{text}%"),
                    startsWith: x => EF.Functions.Like(x.Terminal.ToUpper(), $"{text}%"),
                    endsWith: x => EF.Functions.Like(x.Terminal.ToUpper(), $"%{text}"),
                    lessThan: x => x.Terminal.ToUpper().CompareTo(text) < 0,
                    greaterThan: x => x.Terminal.ToUpper().CompareTo(text) > 0,
                    inValues: null);
            }

            // Gate
            if (filters.Gate != null)
            {
                filters.Gate.Validate();
                string text = filters.Gate.GetSimpleSanitizedValue(sanitizeOptions);
                query = query.WhereTextFilter(filters.Gate.Operator,
                    isNull: null,
                    equalTo: x => x.Gate.ToUpper() == text,
                    like: x => EF.Functions.Like(x.Gate.ToUpper(), $"%{text}%"),
                    startsWith: x => EF.Functions.Like(x.Gate.ToUpper(), $"{text}%"),
                    endsWith: x => EF.Functions.Like(x.Gate.ToUpper(), $"%{text}"),
                    lessThan: x => x.Gate.ToUpper().CompareTo(text) < 0,
                    greaterThan: x => x.Gate.ToUpper().CompareTo(text) > 0,
                    inValues: null);
            }

            // IdList
            if (filters.IdList != null && !filters.IdList.IsEmpty())
            {
                var idList = filters.IdList.GetArrayIds();
                query = query.Where(x => idList.Contains(x.FligthId));
            }

            // IdRange
            if (filters.IdRange != null && filters.IdRange.IsValidRange())
            {
                query = query.Where(x => x.FligthId >= filters.IdRange.MinId && x.FligthId <= filters.IdRange.MaxId);
            }

            return query;
        }

        private IQueryable<Fligth> AddIncludes(IQueryable<Fligth> query, SearchFligthOptions options)
        {
            if (options == null)
                return query;

            query = query
                .Include(x => x.Airline)
                .Include(x => x.DepartureAirport)
                .Include(x => x.ArrivalAirport)
                .Include(x => x.FligthServices);

            return query;
        }
    }
}
