using APIBaseTemplate.Common;

namespace APIBaseTemplate.Datamodel.DTO
{
    /// <summary>
    /// Search request on <see cref="Airport"/> entity
    /// </summary>
    public class SearchAirportRequest : PaginatedSearchRequest<SearchAirportFilters, SearchAirportOptions>
    {

    }

    /// <summary>
    /// Filters about <see cref="Airport"/> entity request
    /// </summary>
    public class SearchAirportFilters : ISearchIdFilters
    {
        /// <summary>
        /// Search on <see cref="Airport.AirportId"/>
        /// </summary>
        public int? AirportId { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.Code"/>
        /// </summary>
        public TextFilter? Code { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.Name"/>
        /// </summary>
        public TextFilter? Name { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.CityId"/>
        /// </summary>
        public int? CityId { get; set; }

        /// <inheritdoc/>
        public IdListFilter IdList { get; set; }

        /// <inheritdoc/>
        public IdRangeFilter IdRange { get; set; }
    }

    /// <summary>
    /// Options about <see cref="Airport"/> entity request
    /// </summary>
    public class SearchAirportOptions
    {

    }
}
