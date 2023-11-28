using APIBaseTemplate.Common;

namespace APIBaseTemplate.Datamodel.DTO
{
    public class SearchCityRequest : PaginatedSearchRequest<SearchCityFilters, SearchCityOptions>
    {

    }

    /// <summary>
    /// Filters about <see cref="City"/> entity request
    /// </summary>
    public class SearchCityFilters : ISearchIdFilters
    {
        /// <summary>
        /// Search on <see cref="City.CityId"/>
        /// </summary>
        public int? CityId { get; set; }

        /// <summary>
        /// Search on <see cref="City.Name"/>
        /// </summary>
        public TextFilter? Name { get; set; }

        /// <summary>
        /// Search on <see cref="Region.RegionId"/>
        /// </summary>
        public int? RegionId { get; set; }

        /// <inheritdoc/>
        public IdListFilter IdList { get; set; }

        /// <inheritdoc/>
        public IdRangeFilter IdRange { get; set; }
    }

    /// <summary>
    /// Options about <see cref="City"/> entity request
    /// </summary>
    public class SearchCityOptions
    {

    }
}
