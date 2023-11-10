namespace APIBaseTemplate.Common
{
    /// <summary>
    /// It describes a response result composed by a list of entity of type <typeparamref name="T"/>
    /// paginati
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedResult<T>
    {
        /// <summary>
        /// Paged elements
        /// </summary>
        public List<T> Items { get; set; } = new List<T>();

        /// <summary>
        /// Sortings criteria if needed.
        /// </summary>
        public List<OrderByOption> Sortings { get; set; } = new List<OrderByOption>();

        /// <summary>
        /// Page index
        /// </summary>
        public int? CurrentPageIndex { get; set; }

        /// <summary>
        /// Page size
        /// </summary>
        public int CurrentPageSize { get; set; }

        /// <summary>
        /// Totale records number without pagination.
        /// </summary>
        public long? TotalRecordsWithoutPagination { get; set; }

        /// <summary>
        /// Total pages considering the current page size.
        /// </summary>
        public long? TotalPages
        {
            get
            {
                if (TotalRecordsWithoutPagination.HasValue && CurrentPageSize >= 1)
                {
                    if (TotalRecordsWithoutPagination.Value > 0)
                    {
                        return (long)Math.Ceiling((double)TotalRecordsWithoutPagination.Value / CurrentPageSize);
                    }
                    else
                    {
                        return 0;
                    }
                }
                else
                {
                    return 0L;
                }
            }
        }

    }
}
