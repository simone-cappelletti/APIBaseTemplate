using APIBaseTemplate.Datamodel.DTO;
using Microsoft.EntityFrameworkCore;
using Airline = APIBaseTemplate.Datamodel.DbEntities.Airline;

namespace APIBaseTemplate.Repositories
{
    public interface IAirlineRepository : IRepository<Airline>
    {
        /// <summary>
        /// Perform a search of <see cref="Airline"/> items using <paramref name="filter"/>
        /// </summary>
        /// <param name="filter"></param>
        IQueryable<Airline> Get(SearchAirlineRequest filter);
    }

    public class AirlineRepository : BaseRepository<Airline>, IAirlineRepository
    {
        private readonly ILogger<AirlineRepository> _logger;

        public AirlineRepository(ILogger<AirlineRepository> logger)
        {
            _logger = logger;
        }

        public IQueryable<Airline> Get(SearchAirlineRequest request)
        {
            IQueryable<Airline> query = this.DbContext.Set<Airline>();

            query = ApplyFilters(query, request.Filters);
            query = AddIncludes(query, request.Options);

            return query;
        }

        private IQueryable<Airline> ApplyFilters(IQueryable<Airline> query, SearchAirlineFilters filters)
        {
            if (filters == null)
                return query;

            // AirlineId
            if (true == filters.AirlineId.HasValue)
            {
                query = query.Where(x => x.AirlineId == filters.AirlineId);
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
