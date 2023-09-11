using System.Text.Json.Serialization;

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
        [JsonPropertyName("airportId")]
        public int? AirportId { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.Code"/>
        /// </summary>
        [JsonPropertyName("code")]
        public string? Code { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.Name"/>
        /// </summary>
        [JsonPropertyName("name")]
        public string? Name { get; set; }

        /// <summary>
        /// Search on <see cref="Airport.CityId"/>
        /// </summary>
        [JsonPropertyName("cityId")]
        public int? CityId { get; set; }
    }

    /// <summary>
    /// Options about <see cref="Airport"/> entity request
    /// </summary>
    public class SearchAirportOptions
    {

    }
}
