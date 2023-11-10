using System.Text.Json.Serialization;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Supported operators for a boolean filter
    /// </summary>
    public enum EnmBooleanFilterOperators
    {
        /// <summary>
        /// Equal to value
        /// </summary>
        EqualTo = 0,

        /// <summary>
        /// Not equal to value
        /// </summary>
        NotEqualTo = 1,

        /// <summary>
        /// Value is null (<see cref="BooleanFilter.Value"/> ignored).
        /// </summary>
        IsNull = 2,

        /// <summary>
        /// Value is not null (<see cref="BooleanFilter.Value"/> ignored).
        /// </summary>
        IsNotNull = 3
    }

    /// <summary>
    /// Boolean filtering field.
    /// </summary>
    public class BooleanFilter : IFilter
    {
        /// <summary>
        /// Operator to be applied.
        /// </summary>
        [JsonPropertyName("operator")]
        public EnmBooleanFilterOperators Operator { get; set; }

        /// <summary>
        /// Value to be used with operator.
        /// </summary>
        [JsonPropertyName("value")]
        public bool Value { get; set; }

        [JsonIgnore]
        public EnmFilterTypes FilterType => EnmFilterTypes.Boolean;

        /// <inheritdoc/>
        public void Validate(string? filterName = null)
        {
            // nothing to validate here
        }
    }
}
