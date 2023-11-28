namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Represents a list of Ids
    /// </summary>
    public class IdListFilter
    {
        /// <summary>
        /// Entity Id List
        /// </summary>
        /// <remarks>The actual Id Type depends on the Entity Type</remarks>
        public List<EntityNumberIdParam> Ids { get; set; } = new();

        /// <summary>
        /// Get array of integer Id of EntityNumberIdParam
        /// </summary>
        /// <returns></returns>
        public int[] GetArrayIds()
        {
            if (this.Ids == null || this.Ids.Count == 0) return Array.Empty<int>();
            return this.Ids.Select(i => i.Id).ToArray();
        }

        /// <summary>
        /// Check if the list of Id is empty
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() => this.Ids == null || this.Ids.Count == 0;

        /// <summary>
        /// Create an EntityIdListFilter from an array of Id
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        public static IdListFilter CreateFrom(params int[] idList)
        {
            var filter = new IdListFilter();
            foreach (var id in idList)
            {
                filter.Ids.Add(new EntityNumberIdParam { Id = id });
            }
            return filter;
        }
    }
}
