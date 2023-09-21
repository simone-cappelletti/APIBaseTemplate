using APIBaseTemplate.Utils;
using System.Text.Json.Serialization;

namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Operator for the text field
    /// </summary>
    public enum EnmTextFilterOperator
    {
        /// <summary>
        /// Like %text% - Contains
        /// </summary>
        Contains = 0,

        /// <summary>
        /// Like text% - Starts with
        /// </summary>
        StartsWith = 1,

        /// <summary>
        /// Like %text - Ends with
        /// </summary>
        EndsWith = 2,

        /// <summary>
        /// Is null - empty (<see cref="TextFilter.Value"/> ignored)
        /// </summary>
        IsNull = 3,

        /// <summary>
        /// = - equals to
        /// </summary>
        EqualTo = 4,

        /// <summary>
        /// &lt; - lower then
        /// </summary>
        LessThan = 5,

        /// <summary>
        /// &gt; - greater then
        /// </summary>
        GreaterThan = 6,

        /// <summary>
        /// Value will match (not case-sensitive) with at least one of provided values
        /// </summary>
        InValues = 7,

        /// <summary>
        /// &lte; lower than or equal to
        /// </summary>
        LessThanEqual = 8,

        /// <summary>
        /// &gte; - greater than or equal to
        /// </summary>
        GreaterThanEqual = 9,

        /// <summary>
        /// NOT Like %text% - not contains
        /// </summary>
        NotContains = 100,

        /// <summary>
        /// NOT Like text% - doesn't start with
        /// </summary>
        NotStartsWith = 101,

        /// <summary>
        /// NOT Like %text - doesen't end with
        /// </summary>
        NotEndsWith = 102,

        /// <summary>
        /// IS NOT NULL - is not null (<see cref="TextFilter.Value"/> ignorato)
        /// </summary>
        IsNotNull = 103,

        /// <summary>
        /// != - different from
        /// </summary>
        NotEqualTo = 104,

        /// <summary>
        /// Value will not match (not case-sensitive) with all provided values
        /// </summary>
        NotInValues = 107,
    }

    /// <summary>
    /// Simple preprocess Operation in text filter
    /// </summary>
    [Flags]
    public enum EnmSimpleTextFilterSanitize
    {
        None = 0,
        RemovePercent = 0x01,
        Trim = 0x02,
        ToUpper = 0x04,
        ToLower = 0x08,
    }

    /// <summary>
    /// Text search field.
    /// </summary>
    public class TextFilter : IFilter
    {
        /// <summary>
        /// Separators used to split multiple string values in <see cref="Value"/> property when
        /// operator is <see cref="EnmTextFilterOperator.InValues"/>
        /// </summary>
        private static readonly string[] STRING_VALUES_SEPARATORS = new string[] { "\n", "\r\n" };

        /// <summary>
        /// Selected oeprator
        /// </summary>
        public EnmTextFilterOperator Operator { get; set; }

        /// <summary>
        /// Value (if any) of the text field for the search
        /// </summary>
        /// <remarks>
        /// May be null (e.g. when operator is <see cref="EnmTextFilterOperator.IsNull"/>).
        /// When operator is <see cref="EnmTextFilterOperator.IsNull"/> this field may contain a list of string values separated by '\n'.
        /// </remarks>
        public string? Value { get; set; }

        ///<inheritdoc/>
        [JsonIgnore]
        public EnmFilterTypes FilterType => EnmFilterTypes.Text;

        /// <summary>
        /// Returns string values contained in <see cref="Value"/>
        /// </summary>
        [JsonIgnore]
        public string[] Values
        {
            get
            {
                if (string.IsNullOrEmpty(Value)) return new string[] { };
                return Value.Split(STRING_VALUES_SEPARATORS, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
            }
        }

        /// <summary>
        /// Returns a simply sanitized copy of the <see cref="Value"/> property
        /// </summary>
        /// <remarks>Original <see cref="Value"/> is not changed </remarks>
        /// <param name="simpleSanitize">sanitize options</param>
        /// <returns>a simply sanitized copy of the <see cref="Value"/> property</returns>
        public string? GetSimpleSanitizedValue(EnmSimpleTextFilterSanitize simpleSanitize) =>
            TextSanitizerHelper.SanitizeTextSimply(this.Value, simpleSanitize);

        private static readonly string[] FILTER_NAMES_SUPPORTING_IN_VALUES_OPERATOR = new string[]
        {

        };

        /// <inheritdoc/>
        public void Validate(string? filterName = null)
        {
            switch (this.Operator)
            {
                case EnmTextFilterOperator.InValues:
                    // if this operator is used
                    // then filter name must be available..
                    if (string.IsNullOrEmpty(filterName))
                        throw new ArgumentException(
                            $"To perform validation, when using operator {this.Operator} then filter name argument must have a value"
                            , nameof(filterName));
                    // ..and filter name should be one of the FILTER_NAMES_SUPPORTING_IN_VALUES_OPERATOR
                    if (!FILTER_NAMES_SUPPORTING_IN_VALUES_OPERATOR.Contains(filterName))
                        throw new FilterException(
                            $"This operator doesn't support this filter. Operator {this.Operator} should be used only to filter {string.Join(',', FILTER_NAMES_SUPPORTING_IN_VALUES_OPERATOR)}",
                            FilterErrorCodes.OPERATOR_NOT_SUPPORTED,
                            this.FilterType,
                            this.Operator
                            );
                    break;

                case EnmTextFilterOperator.Contains:
                case EnmTextFilterOperator.StartsWith:
                case EnmTextFilterOperator.EndsWith:
                case EnmTextFilterOperator.EqualTo:
                case EnmTextFilterOperator.LessThan:
                case EnmTextFilterOperator.GreaterThan:
                case EnmTextFilterOperator.NotContains:
                case EnmTextFilterOperator.NotStartsWith:
                case EnmTextFilterOperator.NotEndsWith:
                case EnmTextFilterOperator.NotEqualTo:
                case EnmTextFilterOperator.NotInValues:
                    if (string.IsNullOrEmpty(this.Value))
                        throw new FilterException(
                            "This filter's operator requires value: cannot be null or empty",
                            FilterErrorCodes.MISSING_VALUE,
                            this.FilterType,
                            this.Operator);
                    break;
                default:
                    break;
            }
        }

    }
}
