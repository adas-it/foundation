namespace Adasit.Foundation.Domain;

/// <summary>
/// Represents an entity that supports soft deletion, allowing it to be marked as deleted without being permanently
/// removed from storage.
/// </summary>
/// <remarks>Soft deletion is a common pattern used to retain historical data while marking entities as logically
/// deleted.  Implementations of this interface should ensure that the entity's state reflects the soft deletion
/// status.</remarks>
public interface ISoftDeletableEntity
{
    /// <summary>
    /// Gets a value indicating whether the entity has been marked as deleted.
    /// </summary>
    bool IsDeleted { get; }

    /// <summary>
    /// Gets the date and time when the entity was deleted.
    /// </summary>
    DateTime DeletedAt { get; }

    /// <summary>
    /// Gets the identifier of the user or entity that performed the deletion.  
    /// </summary>
    string DeletedBy { get; }
}
