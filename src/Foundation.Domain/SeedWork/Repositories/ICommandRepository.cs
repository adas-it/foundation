namespace Adasit.Foundation.Domain.SeedWork.Repositories;

/// <summary>
/// Defines the contract for a command repository in a CQRS (Command Query Responsibility Segregation) architecture.
/// Command repositories handle write operations such as creating, updating, and deleting entities.
/// They are responsible for persisting changes to the data store and ensuring transactional consistency.
/// </summary>
/// <typeparam name="TEntity">The type of entity being managed, which must inherit from <see cref="Entity{TEntityId}"/>.</typeparam>
/// <typeparam name="TEntityId">The type of the entity's identifier, which must implement <see cref="IEquatable{TEntityId}"/> and <see cref="IId{TEntityId}"/>.</typeparam>
/// <remarks>
/// Implementations of this interface should handle the persistence of domain entities and their associated domain events.
/// In CQRS, command repositories are used exclusively for write operations, while query repositories handle read operations.
/// </remarks>
public interface ICommandRepository<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    /// <summary>
    /// Persists the specified entity to the data store asynchronously.
    /// This method handles both insert and update operations based on the entity's state.
    /// </summary>
    /// <param name="entity">The entity to persist.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    /// <remarks>
    /// Implementations should ensure that domain events associated with the entity are also persisted or published.
    /// This method should be transactional to maintain data consistency.
    /// </remarks>
    Task PersistAsync(TEntity entity, CancellationToken cancellationToken);

    /// <summary>
    /// Retrieves an entity by its identifier asynchronously.
    /// This method is typically used to load an existing entity before applying a command operation.
    /// </summary>
    /// <param name="id">The identifier of the entity to retrieve.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task{TResult}"/> containing the entity if found, or null if not found.</returns>
    /// <remarks>
    /// This method is provided for command operations that need to load existing aggregates.
    /// For read-heavy operations, consider using a query repository instead.
    /// </remarks>
    Task<TEntity?> GetByIdAsync(TEntityId id, CancellationToken cancellationToken);
}
