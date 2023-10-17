using System.Text.Json.Serialization;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Date time filtering field
    /// </summary>
    public class DateTimeFilter : BaseAbstractDateFilter, IFilter
    {
        [JsonIgnore]
        public EnmFilterTypes FilterType
            => EnmFilterTypes.DateTime;

        /// <inheritdoc/>
        public void Validate(string? filterName = null)
        {
            switch (this.Operator)
            {
                case EnmDateTimeFilterOperators.Between:
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
