namespace APIBaseTemplate.Common
{
    public enum EnmFilterTypes
    {
        Boolean = 0,
        Date = 1,
        DateTime = 2,
        Number = 3,
        TableValued = 4,
        Text = 5
    }

    /// <summary>
    /// Generic filter interface
    /// </summary>
    public interface IFilter
    {
        /// <summary>
        /// Filter type <see cref="EnmFilterTypes"/>
        /// </summary>
        public EnmFilterTypes FilterType { get; }

        /// <summary>
        /// Validates filter operator and values
        /// </summary>
        /// <exception cref="FilterException"></exception>
        public void Validate(string? filterName = null);
    }
}
