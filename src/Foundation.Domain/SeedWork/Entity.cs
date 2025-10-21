using Adasit.Foundation.Domain.Validations;
using Adasit.Foundation.Domain.ValuesObjects;

namespace Adasit.Foundation.Domain.SeedWork;

/// <summary>
/// Represents a base entity in domain-driven design.
/// Entities are objects that have a distinct identity and are defined by their identity rather than their attributes.
/// This class provides common functionality for validation, notifications, and identity management.
/// </summary>
/// <typeparam name="TEntityId">The type of the entity's identifier, which must implement <see cref="IEquatable{TEntityId}"/> and <see cref="IId{TEntityId}"/>.</typeparam>
/// <remarks>
/// This is an abstract base class that should be inherited by concrete entity classes.
/// It provides a foundation for domain validation and notification handling.
/// </remarks>
public abstract class Entity<TEntityId> where TEntityId : IEquatable<TEntityId>, IId<TEntityId>
{
    /// <summary>
    /// Gets the unique identifier for this entity.
    /// </summary>
    /// <value>The entity's identifier of type <typeparamref name="TEntityId"/>.</value>
    /// <remarks>
    /// The identifier is automatically initialized using <see cref="IId{T}.New()"/> and cannot be changed after initialization.
    /// </remarks>
    public TEntityId Id { get; protected init; } = TEntityId.New();

    private readonly ICollection<Notification> _notifications = [];

    /// <summary>
    /// Gets the read-only collection of validation error notifications for this entity.
    /// </summary>
    /// <value>A read-only collection of <see cref="Notification"/> objects representing validation errors.</value>
    /// <remarks>
    /// Notifications are typically populated during validation operations and indicate issues that prevent the entity from being in a valid state.
    /// </remarks>
    protected IReadOnlyCollection<Notification> Notifications => [.. _notifications];

    private readonly ICollection<Notification> _warnings = [];

    /// <summary>
    /// Gets the read-only collection of validation warning notifications for this entity.
    /// </summary>
    /// <value>A read-only collection of <see cref="Notification"/> objects representing validation warnings.</value>
    /// <remarks>
    /// Warnings indicate potential issues that don't prevent the entity from being valid but may require attention.
    /// </remarks>
    protected IReadOnlyCollection<Notification> Warnings => [.. _warnings];

    private readonly ICollection<Notification> _infos = [];

    /// <summary>
    /// Gets the read-only collection of informational notifications for this entity.
    /// </summary>
    /// <value>A read-only collection of <see cref="Notification"/> objects representing informational messages.</value>
    /// <remarks>
    /// Informational notifications provide additional context or messages that don't affect validation status.
    /// </remarks>
    protected IReadOnlyCollection<Notification> Information => [.. _infos];

    /// <summary>
    /// Performs synchronous validation on the entity.
    /// </summary>
    /// <returns>A <see cref="DomainResult"/> indicating the validation outcome, including any errors or warnings.</returns>
    /// <remarks>
    /// This method validates the entity's identifier and returns a result based on the presence of notifications.
    /// Derived classes can override this method to add custom validation logic.
    /// </remarks>
    protected DomainResult Validate()
    {
        AddNotification(Id!.NotNull());

        return Notifications.Count != 0
            ? DomainResult.Failure(errors: _notifications)
            : DomainResult.Success(warnings: _warnings);
    }

    /// <summary>
    /// Adds a collection of notifications to the entity's error notifications.
    /// </summary>
    /// <param name="notifications">The collection of notifications to add.</param>
    /// <remarks>
    /// Each notification in the collection is added individually using <see cref="AddNotification(Notification?)"/>.
    /// </remarks>
    protected void AddNotification(List<Notification> notifications)
    {
        notifications.ForEach(x => AddNotification(x));
    }

    /// <summary>
    /// Adds a single notification to the entity's error notifications if it is not null.
    /// </summary>
    /// <param name="notification">The notification to add, or null.</param>
    /// <remarks>
    /// Only non-null notifications are added to prevent null entries in the collection.
    /// </remarks>
    protected void AddNotification(Notification? notification)
    {
        if (notification != null)
        {
            _notifications.Add(notification);
        }
    }

    /// <summary>
    /// Creates and adds a new notification to the entity's error notifications.
    /// </summary>
    /// <param name="fieldName">The name of the field that failed validation.</param>
    /// <param name="message">The validation error message.</param>
    /// <param name="domainError">The domain error code associated with this notification.</param>
    /// <remarks>
    /// This is a convenience method for creating and adding notifications in a single call.
    /// </remarks>
    protected void AddNotification(string fieldName, string message, DomainErrorCode domainError)
        => AddNotification(new Notification(fieldName, message, domainError));

    /// <summary>
    /// Adds a single warning notification to the entity if it is not null.
    /// </summary>
    /// <param name="notification">The warning notification to add, or null.</param>
    /// <remarks>
    /// Only non-null notifications are added to prevent null entries in the collection.
    /// </remarks>
    protected void AddWarning(Notification? notification)
    {
        if (notification != null)
        {
            _warnings.Add(notification);
        }
    }

    /// <summary>
    /// Creates and adds a new warning notification to the entity.
    /// </summary>
    /// <param name="fieldName">The name of the field associated with the warning.</param>
    /// <param name="message">The warning message.</param>
    /// <param name="domainError">The domain error code associated with this warning.</param>
    /// <remarks>
    /// This is a convenience method for creating and adding warning notifications in a single call.
    /// </remarks>
    protected void AddWarning(string fieldName, string message, DomainErrorCode domainError)
        => AddWarning(new(fieldName, message, domainError));

    /// <summary>
    /// Adds a single informational notification to the entity if it is not null.
    /// </summary>
    /// <param name="notification">The informational notification to add, or null.</param>
    /// <remarks>
    /// Only non-null notifications are added to prevent null entries in the collection.
    /// </remarks>
    protected void AddInformation(Notification? notification)
    {
        if (notification != null)
        {
            _infos.Add(notification);
        }
    }

    /// <summary>
    /// Creates and adds a new informational notification to the entity.
    /// </summary>
    /// <param name="fieldName">The name of the field associated with the information.</param>
    /// <param name="message">The informational message.</param>
    /// <param name="domainError">The domain error code associated with this information.</param>
    /// <remarks>
    /// This is a convenience method for creating and adding informational notifications in a single call.
    /// </remarks>
    protected void AddInformation(string fieldName, string message, DomainErrorCode domainError)
        => AddInformation(new(fieldName, message, domainError));

    /// <summary>
    /// Performs asynchronous validation on the entity using a validator.
    /// </summary>
    /// <typeparam name="REntity">The type of entity to validate, which must inherit from <see cref="Entity{TEntityId}"/>.</typeparam>
    /// <param name="validator">The validator to use for validation.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A tuple containing the <see cref="DomainResult"/> and the validated entity (or null if validation failed).</returns>
    /// <remarks>
    /// This method uses the provided validator to perform validation and combines the results with the entity's own validation.
    /// If validation fails, the entity is returned as null.
    /// </remarks>
    protected async Task<(DomainResult, REntity?)> ValidateAsync<REntity>(
        IDefaultValidator<REntity, TEntityId> validator,
        REntity entity,
        CancellationToken cancellationToken)
        where REntity : Entity<TEntityId>
    {
        cancellationToken.ThrowIfCancellationRequested();

        var notifications = await validator.ValidateCreationAsync(entity, cancellationToken);

        AddNotification(notifications);
        AddNotification(Id!.NotNull());

        var domainResult = Notifications.Count != 0
            ? DomainResult.Failure(errors: _notifications)
            : DomainResult.Success(warnings: _warnings);

        return domainResult.IsFailure
            ? (domainResult, null)
            : (domainResult, entity);
    }
}
