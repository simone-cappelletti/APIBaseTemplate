namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Represents a range of Ids
    /// </summary>
    public class IdRangeFilter
    {
        /// <summary>
        /// Min Entity Id 
        /// </summary>
        /// <remarks>The actual Id Type depends on the Entity Type</remarks>
        public int MinId { get; set; }

        /// <summary>
        /// Max Entity Id 
        /// </summary>
        /// <remarks>The actual Id Type depends on the Entity Type</remarks>
        public int MaxId { get; set; }

        /// <summary>
        /// Check if the Id range is valid
        /// </summary>
        /// <returns></returns>
        public bool IsValidRange() => MaxId >= MinId;
    }
}
