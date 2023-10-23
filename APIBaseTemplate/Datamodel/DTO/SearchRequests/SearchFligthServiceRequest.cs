using APIBaseTemplate.Common;

namespace APIBaseTemplate.Datamodel.DTO
{
    /// <summary>
    /// Search request on <see cref="FligthService"/> entity
    /// </summary>
    public class SearchFligthServiceRequest : PaginatedSearchRequest<SearchFligthServiceFilters, SearchFligthServiceOptions>
    {

    }

    /// <summary>
    /// Filters about <see cref="FligthService"/> entity request
    /// </summary>
    public class SearchFligthServiceFilters
    {
        /// <summary>
        /// Search on <see cref="FligthService.FligthServiceId"/>
        /// </summary>
        public int? FligthServiceId { get; set; }

        /// <summary>
        /// Search on <see cref="FligthService.FlightServiceType"/>
        /// </summary>
        public FlightServiceType[] FlightServiceType { get; set; }

        /// <summary>
        /// Search on <see cref="FligthService.CurrencyId"/>
        /// </summary>
        public int? CurrencyId { get; set; }

        /// <summary>
        /// Search on <see cref="FligthService.FligthId"/>
        /// </summary>
        public int? FligthId { get; set; }
    }

    /// <summary>
    /// Options about <see cref="FligthService"/> entity request
    /// </summary>
    public class SearchFligthServiceOptions
    {

    }
}
