using APIBaseTemplate.Common;

namespace APIBaseTemplate.Datamodel.DTO
{
    /// <summary>
    /// Search request on <see cref="Fligth"/> entity
    /// </summary>
    public class SearchFligthRequest : PaginatedSearchRequest<SearchFligthFilters, SearchFligthOptions>
    {

    }

    /// <summary>
    /// Filters about <see cref="Fligth"/> entity request
    /// </summary>
    public class SearchFligthFilters
    {
        /// <summary>
        /// Search on <see cref="Fligth.FligthId"/>
        /// </summary>
        public int? FligthId { get; set; }

        /// <summary>
        /// Search on <see cref="Fligth.Code"/>
        /// </summary>
        public TextFilter? Code { get; set; }

        /// <summary>
        /// Search on <see cref="Fligth.AirlineId"/>
        /// </summary>
        public int? AirlineId { get; set; }

        /// <summary>
        /// Search on <see cref="Fligth.DepartureAirportId"/>
        /// </summary>
        public int? DepartureAirportId { get; set; }

        /// <summary>
        /// Search on <see cref="Fligth.ArrivalAirportId"/>
        /// </summary>
        public int? ArrivalAirportId { get; set; }

        /// <summary>
        /// Search on <see cref="Fligth.DepartureTime"/>
        /// </summary>
        public DateTimeFilter? DepartureTime { get; set; }

        /// <summary>
        /// Search on <see cref="Fligth.ArrivalTime"/>
        /// </summary>
        public DateTimeFilter? ArrivalTime { get; set; }

        /// <summary>
        /// Search on <see cref="Fligth.Terminal"/>
        /// </summary>
        public TextFilter? Terminal { get; set; }

        /// <summary>
        /// Search on <see cref="Fligth.Gate"/>
        /// </summary>
        public TextFilter? Gate { get; set; }
    }

    /// <summary>
    /// Options about <see cref="Fligth"/> entity request
    /// </summary>
    public class SearchFligthOptions
    {

    }
}
