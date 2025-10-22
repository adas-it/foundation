using Adasit.Foundation.Domain.SeedWork;
using Adasit.Foundation.Domain.ValuesObjects;

namespace Adasit.Foundation.Domain.Validations;

/// <inheritdoc/>
public class DefaultValidator<TEntity, TEntityId> : IDefaultValidator<TEntity, TEntityId>
    where TEntity : Entity<TEntityId>
    where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    /// <inheritdoc/>
    public virtual async Task<List<Notification>> ValidateCreationAsync(TEntity entity,
        CancellationToken cancellationToken)
    {
        List<Notification> notifications = [];

        await DefaultValidationsAsync(entity, notifications, cancellationToken);

        return notifications;
    }

    /// <summary>
    /// Performs default validations on the entity.
    /// This method can be overridden in derived classes to add additional validation logic.
    /// </summary>
    /// <param name="entity">The entity to validate.</param>
    /// <param name="notifications">The list to add validation notifications to.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    protected virtual Task DefaultValidationsAsync(
        TEntity entity,
        List<Notification> notifications,
        CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ArgumentNullException.ThrowIfNull(entity);
        ArgumentNullException.ThrowIfNull(notifications);

        return Task.CompletedTask;
    }

    /// <summary>
    /// Adds a notification to the list if it is not null.
    /// </summary>
    /// <param name="notification">The notification to add, or null.</param>
    /// <param name="list">The list to add the notification to.</param>
    protected static void AddNotification(Notification? notification, List<Notification> list)
    {
        if (notification != null)
        {
            list.Add(notification);
        }
    }
}
