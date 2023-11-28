namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Common Search Id filters
    /// </summary>
    public interface ISearchIdFilters
    {
        /// <summary>
        /// Search entity with the Id in the list given here
        /// </summary>
        public IdListFilter? IdList { get; set; }

        /// <summary>
        /// Search entity with the Id in the range given here
        /// </summary>
        public IdRangeFilter? IdRange { get; set; }
    }
}
