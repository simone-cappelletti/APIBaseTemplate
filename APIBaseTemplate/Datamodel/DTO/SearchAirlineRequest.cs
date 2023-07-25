using System.Text.Json.Serialization;

namespace APIBaseTemplate.Datamodel.DTO
{
    /// <summary>
    /// Search request on <see cref="Airline"/> entity
    /// </summary>
    public class SearchAirlineRequest : SearchRequest<SearchAirlineFilters, SearchAirlineOptions>
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
        [JsonPropertyName("airlineId")]
        public int? AirlineId { get; set; }

        /// <summary>
        /// Search on <see cref="Airline.Code"/>
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Search on <see cref="Airline.Name"/>
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Search on <see cref="Airline.RegionId"/>
        /// </summary>
        [JsonPropertyName("regionId")]
        public int? RegionId { get; set; }
    }

    /// <summary>
    /// Options about <see cref="Airline"/> entity request
    /// </summary>
    public class SearchAirlineOptions
    {

    }
}
