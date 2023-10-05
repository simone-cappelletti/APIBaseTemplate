using APIBaseTemplate.Common.Requests;
using System.Text.Json.Serialization;

namespace APIBaseTemplate.Common
{
    public class SearchRequest<TFilter, TOptions> : Request, ISorted
    {
        /// <summary>
        /// Specific search filters for this request
        /// </summary>
        public TFilter? Filters { get; set; }

        /// <summary>
        /// Specific options for this request
        /// </summary>
        public TOptions? Options { get; set; }

        /// <summary>
        /// Sorting criteria
        /// </summary>
        public List<OrderByOption> Sortings { get; set; } = new List<OrderByOption>();
    }
}
