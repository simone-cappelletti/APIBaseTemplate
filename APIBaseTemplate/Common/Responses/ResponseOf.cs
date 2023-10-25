using System.Text.Json.Serialization;

namespace APIBaseTemplate.Common
{
    public class ResponseOf<T> : Response
        where T : new()
    {
        /// <summary>
        /// Returning value
        /// </summary>
        [JsonPropertyName("value")]
        public T? Value { get; set; }
    }
}
