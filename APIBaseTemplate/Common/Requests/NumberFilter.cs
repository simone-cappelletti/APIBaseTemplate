using System.Text.Json.Serialization;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Supported operators for a number filter.
    /// </summary>
    public enum EnmNumberFilterOperators
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
        /// Between value and value2
        /// </summary>
        Between = 2,

        /// <summary>
        /// Less than value
        /// </summary>
        LessThan = 3,

        /// <summary>
        /// Greater than value
        /// </summary>
        GreaterThan = 4,

        /// <summary>
        /// Value is null (<see cref="NumberFilter.Value"/> ignored).
        /// </summary>
        IsNull = 5,

        /// <summary>
        /// Value is not null (<see cref="NumberFilter.Value"/> ignored).
        /// </summary>
        IsNotNull = 6
    }

    /// <summary>
    /// Number filtering field
    /// </summary>
    public class NumberFilter : IFilter
    {
        /// <summary>
        /// Operator to be applied
        /// </summary>
        [JsonPropertyName("operator")]
        public EnmNumberFilterOperators Operator { get; set; }

        /// <summary>
        /// Value to be used with operator
        /// </summary>
        [JsonPropertyName("value")]
        public double Value { get; set; }

        /// <summary>
        /// Additional value to be eventually used with operator
        /// (e.g. when using <see cref="EnmNumberFilterOperators.Between"/> operator)
        /// </summary>
        [JsonPropertyName("value2")]
        public double? Value2 { get; set; }

        [JsonIgnore]
        public EnmFilterTypes FilterType => EnmFilterTypes.Number;

        /// <inheritdoc/>
        public void Validate(string? filterName = null)
        {
            switch (this.Operator)
            {
                case EnmNumberFilterOperators.Between:
                    if (!this.Value2.HasValue)
                        throw new FilterException(
                            "When using 'between' operator a second value is required",
                            FilterErrorCodes.MISSING_VALUE,
                            this.FilterType,
                            this.Operator,
                            this.Value);
                    if (this.Value2.Value < this.Value)
                        throw new FilterException(
                            "When using 'between' operator, second value cannot be smallest than first",
                            FilterErrorCodes.INCOHERENT_VALUES,
                            this.FilterType,
                            this.Operator,
                            this.Value,
                            this.Value2.Value
                            );
                    break;
                default:
                    break;
            }
        }
    }
}
