using APIBaseTemplate.Utils;
using System.Text.Json.Serialization;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Supported operators for a date time filter
    /// </summary>
    public enum EnmDateTimeFilterOperators
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
        /// <remarks>
        /// When comparing date/time in db with this operator,
        /// hour part of date should be minimized
        /// </remarks>
        LessThan = 3,

        /// <summary>
        /// Greater than value
        /// </summary>
        /// <remarks>
        /// When comparing date/time in db with this operator,
        /// hour part of date should be maximized
        /// </remarks>
        GreaterThan = 4,

        /// <summary>
        /// Value is null (<see cref="Value"/> ignored)
        /// </summary>
        IsNull = 5,

        /// <summary>
        /// Value is not null (<see cref="Value"/> ignored)
        /// </summary>
        IsNotNull = 6
    }

    public abstract class BaseAbstractDateFilter
    {
        /// <summary>
        /// Operator to be applied
        /// </summary>
        public EnmDateTimeFilterOperators Operator { get; set; }

        /// <summary>
        /// Value to be used with operator
        /// </summary>
        public DateTime Value { get; set; }

        /// <summary>
        /// Additional value to be eventually used with operator
        /// (e.g. when using <see cref="EnmDateTimeFilterOperators.Between"/> operator)
        /// </summary>
        public DateTime? Value2 { get; set; }
    }

    /// <summary>
    /// Date filtering field
    /// </summary>
    public class DateFilter : BaseAbstractDateFilter, IFilter
    {
        [JsonIgnore]
        public EnmFilterTypes FilterType
            => EnmFilterTypes.Date;

        /// <summary>
        /// When using <see cref="EnmDateTimeFilterOperators.LessThan"/> then
        /// <see cref="Value"/> will be modified 00:00:00.000 has hourly part.
        /// When using <see cref="EnmDateTimeFilterOperators.GreaterThan"/> then
        /// <see cref="Value"/> will be modified 23:59:59.999 has hourly part.
        /// When using <see cref="EnmDateTimeFilterOperators.Between"/> then
        /// <see cref="Value"/> will be modified 00:00:00.000 has hourly part and
        /// <see cref="Value2"/> will be modified 23:59:59.999 has hourly part.
        /// </summary>
        public void FixTimeValuesByOperator()
        {
            if (Operator == EnmDateTimeFilterOperators.LessThan)
            {
                Value = DateTimeHelper.MinimizeHourPart(Value);
            }
            else if (Operator == EnmDateTimeFilterOperators.GreaterThan)
            {
                Value = DateTimeHelper.MaximizeHourPart(Value);
            }
            else if (Operator == EnmDateTimeFilterOperators.Between)
            {
                if (!Value2.HasValue)
                    throw new InvalidOperationException(
                        $"With {Operator} a value is required for {nameof(Value2)} property");

                Value = DateTimeHelper.MinimizeHourPart(Value);
                Value2 = DateTimeHelper.MaximizeHourPart(Value2.Value);
            }
        }

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
