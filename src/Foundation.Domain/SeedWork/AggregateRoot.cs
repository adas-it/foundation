using Adasit.Foundation.Domain.Events;

namespace Adasit.Foundation.Domain.SeedWork;

/// <summary>
/// Represents an aggregate root in domain-driven design.
/// Aggregate roots are the top-level entities that define transactional boundaries and maintain domain invariants.
/// They encapsulate domain logic and are responsible for publishing domain events when significant business events occur.
/// </summary>
/// <typeparam name="T">The type of the aggregate root's identifier, which must implement <see cref="IEquatable{T}"/> and <see cref="IId{T}"/>.</typeparam>
/// <remarks>
/// Aggregate roots should be the only entities modified directly by application services.
/// They ensure consistency within their boundaries and publish events to communicate changes to other parts of the system.
/// </remarks>
public abstract class AggregateRoot<T> : Entity<T>, IAggregateRoot where T : IEquatable<T>, IId<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AggregateRoot{T}"/> class.
    /// </summary>
    /// <remarks>
    /// This constructor initializes the internal collection for storing domain events.
    /// </remarks>
    protected AggregateRoot()
    {
        _events = new HashSet<DomainEvent>();
    }

    private readonly ICollection<DomainEvent> _events;

    /// <summary>
    /// Gets the read-only collection of domain events that have occurred within this aggregate root.
    /// </summary>
    /// <value>A read-only collection of <see cref="DomainEvent"/> objects.</value>
    /// <remarks>
    /// <inheritdoc cref="IAggregateRoot.Events"/>
    /// </remarks>
    public IReadOnlyCollection<DomainEvent> Events => [.. _events];

    /// <summary>
    /// Clears all domain events from the aggregate root.
    /// </summary>
    /// <remarks>
    /// <inheritdoc cref="IAggregateRoot.ClearEvents"/>
    /// </remarks>
    public void ClearEvents()
    {
        _events.Clear();
    }

    /// <summary>
    /// Raises a domain event by adding it to the internal collection of events.
    /// </summary>
    /// <param name="event">The domain event to raise.</param>
    /// <remarks>
    /// Domain events should be raised when significant business events occur within the aggregate.
    /// The events will be published after the aggregate's changes are persisted.
    /// </remarks>
    protected void RaiseDomainEvent(DomainEvent @event)
    {
        _events.Add(@event);
    }
}
