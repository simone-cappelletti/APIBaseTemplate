using System.Text.Json.Serialization;

namespace APIBaseTemplate.Datamodel.DTO
{
    public class SearchRequest<TFilter, TOptions>
    {
        /// <summary>
        /// Specific search filters for this request
        /// </summary>
        [JsonPropertyName("filters")]
        public TFilter? Filters { get; set; }

        /// <summary>
        /// Specific options for this request
        /// </summary>
        [JsonPropertyName("options")]
        public TOptions? Options { get; set; }
    }
}
