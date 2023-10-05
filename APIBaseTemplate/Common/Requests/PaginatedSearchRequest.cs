namespace APIBaseTemplate.Common
{
    public class PaginatedSearchRequest<TFilter, TOptions> : SearchRequest<TFilter, TOptions>, IPaginated
    {
        /// <summary>
        /// Page index
        /// </summary>
        public int? PageIndex { get; set; } = 0;

        /// <summary>
        /// Page size
        /// </summary>
        public int PageSize { get; set; } = 10;
    }
}
