namespace APIBaseTemplate.Common
{
    /// <summary>
    /// Represents a general purpose parameter to retrieve a single entity by its Id.
    /// The actual Id Type depends on the Entity Key Type
    /// </summary>
    /// <remarks>
    /// </remarks>
    public interface IEntityNumberIdParam
    {
        /// <summary>
        /// Entity Id
        /// </summary>
        /// <remarks>The actual Id Type depends on the Entity Key Type</remarks>
        int Id { get; set; }
    }
}
