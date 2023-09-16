using APIBaseTemplate.Datamodel.DTO;
using Microsoft.EntityFrameworkCore;
using Airport = APIBaseTemplate.Datamodel.DbEntities.Airport;

namespace APIBaseTemplate.Repositories
{
    public interface IAirportRepository : IRepository<Airport>
    {
        /// <summary>
        /// Perform a search of <see cref="Airport"/> items using <paramref name="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        IQueryable<Airport> Get(SearchAirportRequest filter);
    }

    public class AirportRepository : BaseRepository<Airport>, IAirportRepository
    {
        private readonly ILogger<AirportRepository> _logger;

        public AirportRepository(ILogger<AirportRepository> logger)
        {
            _logger = logger;
        }

        public IQueryable<Airport> Get(SearchAirportRequest request)
        {
            IQueryable<Airport> query = this.DbContext.Set<Airport>();

            query = ApplyFilters(query, request.Filters);
            query = AddIncludes(query, request.Options);

            return query;
        }

        private IQueryable<Airport> ApplyFilters(IQueryable<Airport> query, SearchAirportFilters filters)
        {
            if (filters == null)
                return query;

            // AirportId
            if (true == filters.AirportId.HasValue)
            {
                query = query.Where(x => x.AirportId == filters.AirportId);
            }

            // Code
            if (false == string.IsNullOrWhiteSpace(filters.Code))
            {
                query = query.Where(x => x.Code == filters.Code);
            }

            // Name
            if (false == string.IsNullOrWhiteSpace(filters.Name))
            {
                query = query.Where(x => x.Name == filters.Name);
            }

            // CityId
            if (false == filters.CityId.HasValue)
            {
                query = query.Where(x => x.CityId == filters.CityId);
            }

            return query;
        }

        private IQueryable<Airport> AddIncludes(IQueryable<Airport> query, SearchAirportOptions options)
        {
            if (options == null)
                return query;

            query = query.Include(x => x.City);

            return query;
        }
    }
}
