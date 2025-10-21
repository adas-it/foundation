using Adasit.Foundation.Domain.SeedWork;
using Adasit.Foundation.Domain.ValuesObjects;

namespace Adasit.Foundation.Domain.Validations;

/// <summary>
/// Provides a base implementation of <see cref="IDefaultValidator{TEntity, TEntityId}"/> for validating entity creation.
/// This class serves as a foundation for domain-specific validators and can be extended to add custom validation logic.
/// </summary>
/// <typeparam name="TEntity">The type of entity being validated, which must inherit from <see cref="Entity{TEntityId}"/>.</typeparam>
/// <typeparam name="TEntityId">The type of the entity's identifier, which must implement <see cref="IEquatable{TEntityId}"/> and <see cref="IId{TEntityId}"/>.</typeparam>
public interface IDefaultValidator<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    /// <summary>
    /// Validates the creation of an entity asynchronously.
    /// This method performs default validations and can be overridden in derived classes to add custom validation logic.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A list of <see cref="Notification"/> objects representing validation errors. An empty list indicates successful validation.</returns>
    Task<List<Notification>> ValidateCreationAsync(TEntity entity,
        CancellationToken cancellationToken);
}
