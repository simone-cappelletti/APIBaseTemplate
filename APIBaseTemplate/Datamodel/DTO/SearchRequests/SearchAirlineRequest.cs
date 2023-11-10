using APIBaseTemplate.Common;

namespace APIBaseTemplate.Datamodel.DTO
{
    /// <summary>
    /// Search request on <see cref="Airline"/> entity
    /// </summary>
    public class SearchAirlineRequest : PaginatedSearchRequest<SearchAirlineFilters, SearchAirlineOptions>
    {

    }

    /// <summary>
    /// Filters about <see cref="Airline"/> entity request
    /// </summary>
    public class SearchAirlineFilters
    {
        /// <summary>
        /// Search on <see cref="Airline.AirlineId"/>
        /// </summary>
        public int? AirlineId { get; set; }

        /// <summary>
        /// Search on <see cref="Airline.Code"/>
        /// </summary>
        public TextFilter? Code { get; set; }

        /// <summary>
        /// Search on <see cref="Airline.Name"/>
        /// </summary>
        public TextFilter? Name { get; set; }

        /// <summary>
        /// Search on <see cref="Airline.RegionId"/>
        /// </summary>
        public int? RegionId { get; set; }
    }

    /// <summary>
    /// Options about <see cref="Airline"/> entity request
    /// </summary>
    public class SearchAirlineOptions
    {

    }
}
