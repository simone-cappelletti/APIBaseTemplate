namespace APIBaseTemplate.Datamodel.Interfaces
{
    /// <summary>
    /// Interface to support logical deletion
    /// </summary>
    public interface IDeletableEntity
    {
        bool IsDeleted { get; set; }
    }
}
