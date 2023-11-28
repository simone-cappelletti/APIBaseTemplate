using APIBaseTemplate.Common;

namespace APIBaseTemplate.Datamodel.DTO
{
    /// <summary>
    /// Search request on <see cref="Region"/> entity
    /// </summary>
    public class SearchRegionRequest : PaginatedSearchRequest<SearchRegionFilters, SearchRegionOptions>
    {

    }

    /// <summary>
    /// Filters about <see cref="Region"/> entity request
    /// </summary>
    public class SearchRegionFilters : ISearchIdFilters
    {
        /// <summary>
        /// Search on <see cref="Region.RegionId"/>
        /// </summary>
        public int? RegionId { get; set; }

        /// <summary>
        /// Search on <see cref="Region.Name"/>
        /// </summary>
        public TextFilter? Name { get; set; }

        /// <inheritdoc/>
        public IdListFilter IdList { get; set; }

        /// <inheritdoc/>
        public IdRangeFilter IdRange { get; set; }
    }

    /// <summary>
    /// Options about <see cref="Region"/> entity request
    /// </summary>
    public class SearchRegionOptions
    {

    }
}
