using System.Text.Json.Serialization;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Represents the result of an operation performed by a remote service that does not return any value.
    /// </summary>
    /// <remarks>
    /// In case of error will be returned <see cref="ErrorDescriptor"/>
    /// </remarks>
    public class Response
    {
        /// <summary>
        /// JSON generation UTC timestamp on backend
        /// </summary>
        [JsonPropertyName("tsUtc")]
        public DateTime TimeStampUtc { get; set; } = DateTime.UtcNow;
    }
}
