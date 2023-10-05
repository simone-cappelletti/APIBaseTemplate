using APIBaseTemplate.Common;
using APIBaseTemplate.Common.Requests;

namespace APIBaseTemplate.Datamodel.DTO
{
    /// <summary>
    /// Search request on <see cref="Airport"/> entity
    /// </summary>
    public class SearchAirportRequest : SearchRequest<SearchAirportFilters, SearchAirportOptions>
    {

    }

    /// <summary>
    /// Filters about <see cref="Airport"/> entity request
    /// </summary>
    public class SearchAirportFilters
    {
        /// <summary>
        /// Search on <see cref="Airport.AirportId"/>
        /// </summary>
        public int? AirportId { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.Code"/>
        /// </summary>
        public TextFilter Code { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.Name"/>
        /// </summary>
        public TextFilter Name { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.CityId"/>
        /// </summary>
        public int? CityId { get; set; }
    }

    /// <summary>
    /// Options about <see cref="Airport"/> entity request
    /// </summary>
    public class SearchAirportOptions
    {

    }
}
